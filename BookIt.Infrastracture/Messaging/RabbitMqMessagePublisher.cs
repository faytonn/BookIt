using System.Text;
using System.Text.Json;
using BookIt.Application.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BookIt.Infrastracture.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RabbitMqConnection _connection;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;

    public RabbitMqMessagePublisher(RabbitMqConnection connection, ILogger<RabbitMqMessagePublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(message, JsonOptions);
            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Type = typeof(T).Name,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(_connection.Options.Exchange, routingKey, mandatory: false, properties, body, cancellationToken);
            _logger.LogInformation("Published {MessageType} to {Exchange}/{RoutingKey}", typeof(T).Name, _connection.Options.Exchange, routingKey);
        }
        catch (Exception ex)
        {
            // The sale is already committed; losing a notification must not fail the request.
            _logger.LogError(ex, "Publishing {MessageType} to RabbitMQ failed", typeof(T).Name);
        }
    }
}
