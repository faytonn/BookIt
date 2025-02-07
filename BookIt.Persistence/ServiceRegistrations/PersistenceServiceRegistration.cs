using BookIt.Persistence.Contexts;
using BookIt.Persistence.DataInitializers;
using BookIt.Persistence.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using BookIt.Domain.Entities;

namespace BookIt.Persistence.ServiceRegistrations;

public static class PersistenceServiceRegistration
{

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<BaseEntityInterceptor>();
        services.AddScoped<DbContextInitializer>();


        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


        _addLocalizers(services);

        services.AddHttpClient();

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddHttpContextAccessor();

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
    }
}
