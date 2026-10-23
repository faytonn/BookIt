using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace BookIt.Presentation.Api;

/// <summary>
/// Per-client fixed-window limit on the hold endpoint. On-sale traffic is dominated by
/// scripts hammering "hold" for the same seats; capping each IP keeps one bot from
/// eating the whole capacity and keeps the database from doing work it will reject anyway.
/// </summary>
public static class HoldRateLimiting
{
    public const string PolicyName = "holds";

    public class Options
    {
        public const string SectionName = "RateLimiting:Holds";

        /// <summary>Requests allowed per window per client IP.</summary>
        public int PermitLimit { get; set; } = 20;

        /// <summary>Window length in seconds.</summary>
        public int WindowSeconds { get; set; } = 10;
    }

    public static IServiceCollection AddHoldRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(Options.SectionName).Get<Options>() ?? new Options();

        services.AddRateLimiter(limiter =>
        {
            limiter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiter.AddPolicy(PolicyName, context =>
            {
                var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = options.PermitLimit,
                    Window = TimeSpan.FromSeconds(options.WindowSeconds),
                    QueueLimit = 0
                });
            });

            limiter.OnRejected = (context, _) =>
            {
                context.HttpContext.Response.Headers.RetryAfter = options.WindowSeconds.ToString();
                return ValueTask.CompletedTask;
            };
        });

        return services;
    }
}
