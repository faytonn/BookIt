using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BookIt.Infrastracture.Messaging;

/// <summary>
/// One long-lived AMQP connection for the process (connections are expensive, channels are cheap).
/// Created lazily so a broker that is down at startup only affects messaging, not the web app.
/// </summary>
public sealed class RabbitMqConnection : IAsyncDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqConnection> _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IConnection? _connection;

    public RabbitMqConnection(IOptions<RabbitMqOptions> options, ILogger<RabbitMqConnection> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public RabbitMqOptions Options => _options;

    public bool IsOpen => _connection?.IsOpen == true;

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection?.IsOpen == true)
            return _connection;

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (_connection?.IsOpen == true)
                return _connection;

            if (_connection != null)
                await _connection.DisposeAsync();

            var factory = new ConnectionFactory
            {
                HostName = _options.Host!,
                Port = _options.Port,
                UserName = _options.User,
                Password = _options.Password,
                AutomaticRecoveryEnabled = true,
                ClientProvidedName = "bookit"
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}", _options.Host, _options.Port);
            return _connection;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <summary>Declares the exchange (idempotent) on a fresh channel and returns that channel.</summary>
    public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
    {
        var connection = await GetConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(_options.Exchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);
        return channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
            await _connection.DisposeAsync();
        _gate.Dispose();
    }
}
