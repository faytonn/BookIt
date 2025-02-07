using BookIt.Application.Interfaces.Services.External;
using BookIt.Infrastracture.External;
using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Infrastracture;

public static class InfrastractureServiceRegistration
{
    public static IServiceCollection AddInfrastractureServices(this IServiceCollection services)
    {
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
