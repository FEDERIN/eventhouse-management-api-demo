using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence;

public sealed class ManagementDbContext(DbContextOptions<ManagementDbContext> options)
    : DbContext(options)
{
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<Artist> Artists { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<Venue> Venues { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);
    }
}
