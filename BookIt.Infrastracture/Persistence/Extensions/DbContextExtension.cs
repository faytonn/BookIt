using Microsoft.EntityFrameworkCore;

namespace BookIt.Infrastracture.Persistence.Extensions
{
    public static class DbContextExtensions
    {
        public static void SoftDelete<T>(this DbContext context, T entity) where T : class
        {
            var property = context.Entry(entity).Property("IsDeleted");
            if (property != null)
            {
                property.CurrentValue = true;
                context.Entry(entity).Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        public static IQueryable<T> IgnoreSoftDelete<T>(this IQueryable<T> query) where T : class
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "e");
            var body = System.Linq.Expressions.Expression.Constant(true);
            var expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, parameter);

            return query.IgnoreQueryFilters();
        }
    }
}
