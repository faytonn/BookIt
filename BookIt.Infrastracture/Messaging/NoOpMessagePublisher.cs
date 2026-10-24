using BookIt.Application.Interfaces.Messaging;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastracture.Messaging;

/// <summary>Used when RabbitMq:Host is not configured (local dev without a broker, tests).</summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NoOpMessagePublisher> _logger;

    public NoOpMessagePublisher(ILogger<NoOpMessagePublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Messaging disabled; dropped {MessageType} ({RoutingKey})", typeof(T).Name, routingKey);
        return Task.CompletedTask;
    }
}
