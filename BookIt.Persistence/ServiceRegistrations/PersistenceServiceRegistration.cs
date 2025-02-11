using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.UI;
using BookIt.Domain.Entities;
using BookIt.Infrastracture.Implementations.Services;
using BookIt.Infrastracture.Localizers;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.DataInitializers;
using BookIt.Persistence.Helpers;
using BookIt.Persistence.Implementations.Repositories;
using BookIt.Persistence.Implementations.Services;
using BookIt.Persistence.Implementations.Services.UI;
using BookIt.Persistence.Interceptors;
using BookIt.Persistence.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace BookIt.Persistence.ServiceRegistrations;

public static class PersistenceServiceRegistration
{

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<BaseEntityInterceptor>();
        services.AddScoped<DbContextInitializer>();

        _addRepositories(services);
        _addServices(services);

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

            options.SignIn.RequireConfirmedEmail = true;

            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<CustomIdentityErrorDescriber>();

        services.AddHttpClient();

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddHttpContextAccessor();

        return services;
    }


    private static void _addRepositories(IServiceCollection services)
    {
        services.AddScoped<ICancellationRefundRepository, CancellationRefundRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryDetailRepository, CategoryDetailRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IEventDetailRepository, EventDetailRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventDetailSeatTypeRepository, EventSeatTypeRepository>();
        services.AddScoped<IGeneralLocationRepository, GeneralLocationRepository>();
        services.AddScoped<IHallRepository, HallRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<INewsDetailRepository, NewsDetailRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<INotificationDetailRepository, NotificationDetailRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationSeatRepository, ReservationSeatRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<ISettingDetailRepository, SettingDetailRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();
        services.AddScoped<IWaitlistEntryRepository, WaitlistEntryRepository>();
    }

    private static void _addServices(IServiceCollection services)
    {
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ILayoutService, LayoutService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IGeneralLocationService, GeneralLocationService>();
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventDetailService, EventDetailService>();
        services.AddScoped<IEventDetailSeatTypeService, EventDetailSeatTypeService>();
        services.AddScoped<ISeatTypeService, SeatTypeService>();
        services.AddScoped<ISeatService, SeatService>();
    }
}
