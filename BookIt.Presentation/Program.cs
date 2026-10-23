using BookIt.Application.ServiceRegistrations;
using BookIt.Infrastracture;
using BookIt.Persistence.DataInitializers;
using BookIt.Persistence.ServiceRegistrations;
using BookIt.Persistence.Contexts;
using BookIt.Presentation.Api;
using BookIt.Presentation.Filters;
using BookIt.Presentation.Health;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace BookIt.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //builder.Services.AddDbContext<AppDbContext>(options =>
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        


        // Add services to the container.
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<NotFoundExceptionFilter>();
        });

        builder.Services.AddApplicationServices();
        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddInfrastractureServices();

        builder.Services.AddProblemDetails();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("sqlserver", tags: [HealthEndpoints.ReadyTag]);

        builder.Services.AddHoldRateLimiting(builder.Configuration);

        // API callers get status codes, browser pages still get the login redirect
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });


        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddMvc().AddViewLocalization();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var dataInitializer = scope.ServiceProvider.GetRequiredService<DbContextInitializer>();

            await dataInitializer.InitDatabaseAsync();

            // `dotnet run -- seed-loadtest`: create the 5,000-seat event + user pool and exit
            if (args.Contains("seed-loadtest"))
            {
                var seeder = scope.ServiceProvider.GetRequiredService<LoadTestSeeder>();
                var eventId = await seeder.SeedAsync();
                Console.WriteLine($"Load-test event id: {eventId}");
                return;
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions!.Value);


        app.UseRouting();

        app.UseRateLimiter();

        // Add authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapHealthEndpoints();
        app.MapSeatApi();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        await app.RunAsync();
    }
}
