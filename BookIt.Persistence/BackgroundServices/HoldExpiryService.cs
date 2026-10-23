using BookIt.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookIt.Persistence.BackgroundServices;

/// <summary>
/// Releases expired seat holds once a minute. Reads treat an expired hold as free
/// already, so this sweep is only housekeeping; it is safe to run late or twice.
/// </summary>
public class HoldExpiryService : BackgroundService
{
    public static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<HoldExpiryService> _logger;

    public HoldExpiryService(IServiceScopeFactory scopeFactory, ILogger<HoldExpiryService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _releaseExpiredHoldsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Releasing expired seat holds failed");
            }

            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task _releaseExpiredHoldsAsync()
    {
        // BackgroundService is a singleton; scoped services (DbContext) need their own scope per run.
        using var scope = _scopeFactory.CreateScope();
        var eventSeatService = scope.ServiceProvider.GetRequiredService<IEventSeatService>();

        var released = await eventSeatService.ReleaseExpiredHoldsAsync();
        if (released > 0)
        {
            _logger.LogInformation("Released {Count} expired seat holds", released);
        }
    }
}
