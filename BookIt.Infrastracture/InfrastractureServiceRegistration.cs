using BookIt.Application.Interfaces.Helpers;
using BookIt.Application.Interfaces.Localizers;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Application.Interfaces.Services.UI;
using BookIt.Application.Interfaces.Services;
using BookIt.Infrastracture.External;
using BookIt.Infrastracture.Localizers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using BookIt.Infrastracture.Implementations.Services;

namespace BookIt.Infrastracture;

public static class InfrastractureServiceRegistration
{
    public static IServiceCollection AddInfrastractureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IValidationMessagesProvider, ValidationMessagesLocalizer>();

        _addLocalizers(services);

        return services;
    }


    
    private static void _addLocalizers(IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(
          options =>
          {
              var supportedCultures = new List<CultureInfo>
                  {
                        new CultureInfo("en"),
                        new CultureInfo("aze"),
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
