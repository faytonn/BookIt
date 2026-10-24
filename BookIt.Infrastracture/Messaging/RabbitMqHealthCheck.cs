using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookIt.Infrastracture.Messaging;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly RabbitMqConnection _connection;

    public RabbitMqHealthCheck(RabbitMqConnection connection)
    {
        _connection = connection;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = await _connection.GetConnectionAsync(cancellationToken);
            return connection.IsOpen
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Connection is closed");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Cannot connect to RabbitMQ", ex);
        }
    }
}
