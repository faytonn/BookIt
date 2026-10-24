namespace BookIt.Application.Interfaces.Messaging;

public interface IMessagePublisher
{
    /// <summary>Fire-and-forget publish; must never throw into the caller's request.</summary>
    Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default);
}
