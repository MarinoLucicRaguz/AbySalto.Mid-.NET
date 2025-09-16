using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.WebApi.Extensions
{
    public static class MigrationChecker
    {
        public static void VerifyMigrations<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            var pending = context.Database.GetPendingMigrations().ToList();
            if (pending.Any())
            {
                throw new InvalidOperationException($"Database is not up to date. Pending migrations: {string.Join(", ", pending)}");
            }
        }
    }
}
