using System.Text.Json;
using BookIt.Application.DTOs.EmailDTO;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Application.Messaging;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookIt.Infrastracture.Messaging;

/// <summary>
/// Consumes SeatSoldMessage and sends the buyer a confirmation email. Runs out of the
/// request path so a slow SMTP server never slows down the on-sale. Messages are acked
/// only after the email went out; a failure is logged and the message dropped (no
/// infinite redelivery of a poison message).
/// </summary>
public class SeatSoldEmailConsumer : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(10);

    private readonly RabbitMqConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SeatSoldEmailConsumer> _logger;
    private readonly string? _publicBaseUrl;

    public SeatSoldEmailConsumer(RabbitMqConnection connection, IServiceScopeFactory scopeFactory, ILogger<SeatSoldEmailConsumer> logger, IConfiguration configuration)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;
        // emails need absolute links; without this setting the email just shows the booking code
        _publicBaseUrl = configuration["App:PublicBaseUrl"]?.TrimEnd('/');
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _consumeAsync(stoppingToken);
                return;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ consumer failed; retrying in {Delay}s", RetryDelay.TotalSeconds);
                await Task.Delay(RetryDelay, stoppingToken);
            }
        }
    }

    private async Task _consumeAsync(CancellationToken stoppingToken)
    {
        var options = _connection.Options;
        var channel = await _connection.CreateChannelAsync(stoppingToken);

        await channel.QueueDeclareAsync(options.SeatSoldEmailQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(options.SeatSoldEmailQueue, options.Exchange, SeatSoldMessage.RoutingKey, cancellationToken: stoppingToken);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 5, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var message = JsonSerializer.Deserialize<SeatSoldMessage>(ea.Body.Span, JsonOptions)
                              ?? throw new InvalidOperationException("Empty SeatSoldMessage");
                await _sendConfirmationAsync(message, stoppingToken);
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Handling SeatSoldMessage failed; message dropped");
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(options.SeatSoldEmailQueue, autoAck: false, consumer, stoppingToken);
        _logger.LogInformation("Consuming {Queue}", options.SeatSoldEmailQueue);

        // keep the channel alive until shutdown
        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }

    private async Task _sendConfirmationAsync(SeatSoldMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var user = await userManager.FindByIdAsync(message.UserId);
        if (user?.Email == null)
        {
            _logger.LogWarning("No email for user {UserId}; skipping confirmation for reservation {ReservationId}", message.UserId, message.ReservationId);
            return;
        }

        var reference = message.BookingCode ?? $"#{message.ReservationId}";
        var ticketsLink = message.BookingCode != null && _publicBaseUrl != null
            ? $"""<p><a href="{_publicBaseUrl}/tickets/{message.BookingCode}">Open your ticket (QR code for the door)</a></p>"""
            : "<p>Your ticket with its QR code is under My Profile on BookIt.</p>";

        var body = $"""
            <h2>Your ticket is confirmed</h2>
            <p>Hi {user.FirstName},</p>
            <p>Seat <strong>{message.SeatName}</strong> for <strong>{message.EventTitle}</strong>
               on {message.EventDate:dddd, d MMMM yyyy HH:mm} is yours.</p>
            <p>Price: €{message.Price:0.00} &middot; Booking <strong>{reference}</strong></p>
            {ticketsLink}
            <p>See you there!<br/>BookIt</p>
            """;

        await emailService.SendEmailAsync(new SendEmailDTO
        {
            ToEmail = user.Email,
            Subject = $"Ticket confirmed: {message.EventTitle}",
            Body = body
        });

        _logger.LogInformation("Confirmation email sent for reservation {ReservationId} to {Email}", message.ReservationId, user.Email);
    }
}
