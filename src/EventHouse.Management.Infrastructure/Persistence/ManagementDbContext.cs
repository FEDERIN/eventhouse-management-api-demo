using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence;

public class ManagementDbContext(DbContextOptions<ManagementDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<ArtistGenre> ArtistGenres { get; set; }
    public DbSet<SeatingMap> SeatingMaps { get; set; }
    public DbSet<EventVenue> EventVenues { get; set; }
    public DbSet<EventVenueCalendar> EventVenueCalendars { get; set; }
    public DbSet<ArtistPerformance> ArtistPerformances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);

        // Observe that PostgreSQL's "timestamp with time zone" type is used for all DateTime properties to ensure proper handling of time zones.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var property in properties)
            {
                property.SetColumnType("timestamp with time zone");
            }
        }
    }
}