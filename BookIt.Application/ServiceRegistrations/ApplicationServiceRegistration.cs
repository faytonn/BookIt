using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookIt.Application.ServiceRegistrations;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //services.AddFluentValidationAutoValidation();
        //services.AddValidatorsFromAssemblyContaining(typeof(SettingUpdateDtoValidator));

        return services;
    }
}
