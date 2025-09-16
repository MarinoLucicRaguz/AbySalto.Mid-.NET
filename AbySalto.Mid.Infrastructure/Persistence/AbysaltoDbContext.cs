using AbySalto.Mid.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Persistence
{
    public class AbysaltoDbContext : DbContext
    {
        public AbysaltoDbContext(DbContextOptions<AbysaltoDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbysaltoDbContext).Assembly);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Basket> Basket => Set<Basket>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
