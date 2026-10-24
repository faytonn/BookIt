using BookIt.Application.Interfaces.Helpers;
using BookIt.Application.Interfaces.Messaging;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Infrastracture.External;
using BookIt.Infrastracture.Implementations.Services;
using BookIt.Infrastracture.Localizers;
using BookIt.Infrastracture.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace BookIt.Infrastracture;

public static class InfrastractureServiceRegistration
{
    public static IServiceCollection AddInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IValidationMessagesProvider, ValidationMessagesLocalizer>();

        _addLocalizers(services);
        _addMessaging(services, configuration);

        return services;
    }

    private static void _addMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        var options = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>() ?? new RabbitMqOptions();

        if (!options.Enabled)
        {
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
            return;
        }

        services.AddSingleton<RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<SeatSoldEmailConsumer>();
        services.AddHealthChecks().AddCheck<RabbitMqHealthCheck>("rabbitmq", tags: ["ready"]);
    }

    private static void _addLocalizers(IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(
          options =>
          {
              var supportedCultures = new List<CultureInfo>
                  {
                        new CultureInfo("en"),
                        new CultureInfo("az"),
                        new CultureInfo("cs")
                  };

              options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");

              options.SupportedCultures = supportedCultures;
              options.SupportedUICultures = supportedCultures;

          });

        services.AddSingleton<LayoutLocalizer>();
        services.AddSingleton<ValidationMessagesLocalizer>();
    }
}
