using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace BookIt.Persistence.ServiceRegistrations;

public static class PersistenceServiceRegistration
{

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
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
                        new CultureInfo("az"),
                        new CultureInfo("cs")
                  };

              //options.DefaultRequestCulture = new RequestCulture(culture: "az", uiCulture: "az");

              //options.SupportedCultures = supportedCultures;
              //options.SupportedUICultures = supportedCultures;

          });
    }
}
