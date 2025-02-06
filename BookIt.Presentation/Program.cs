using BookIt.Persistence.Contexts;
using BookIt.Persistence.DataInitializers;
using BookIt.Persistence.Interceptors;
using BookIt.Persistence.ServiceRegistrations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        


        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddPersistenceServices(builder.Configuration);

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddMvc().AddViewLocalization();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var dataInitializer = scope.ServiceProvider.GetRequiredService<DbContextInitializer>();

            await dataInitializer.InitDatabaseAsync();
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

        app.UseAuthorization();


        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}
