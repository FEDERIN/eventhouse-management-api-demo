using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence
{
    public class ManagementDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);
        }
    }
}
