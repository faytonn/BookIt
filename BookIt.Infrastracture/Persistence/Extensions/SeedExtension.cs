using BookIt.Infrastracture.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BookIt.Infrastracture.Persistence.Seeder;

namespace BookIt.Infrastracture.Persistence.Extensions
{
    public static class SeedExtensions
    {

        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            var seeder = new DatabaseSeeder(context);
            await seeder.SeedAsync();
        }
    }
}
