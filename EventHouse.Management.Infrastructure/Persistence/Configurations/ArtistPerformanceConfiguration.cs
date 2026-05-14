using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class ArtistPerformanceConfiguration : IEntityTypeConfiguration<ArtistPerformance>
{
    public void Configure(EntityTypeBuilder<ArtistPerformance> builder)
    {
        builder.ToTable("ArtistPerformances", t =>
        {
            t.HasCheckConstraint("CK_ArtistPerformance_ArtistId_NotEmpty", "ArtistId <> '00000000-0000-0000-0000-000000000000'");
            t.HasCheckConstraint("CK_ArtistPerformance_EventVenueCalendarId_NotEmpty", "EventVenueCalendarId <> '00000000-0000-0000-0000-000000000000'");
        });

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.EventVenueCalendarId)
            .IsRequired();

        builder.Property(e => e.ArtistId)
            .IsRequired();

        builder.Property(e => e.IsHeadliner)
            .IsRequired();

        builder.Property(e => e.SetStart);
        builder.Property(e => e.SetEnd);

        builder.HasIndex(e => new { e.ArtistId, e.EventVenueCalendarId })
            .IsUnique()
            .HasDatabaseName("UX_ArtistPerformances_Artist_EventVenueCalendar");

        builder.HasIndex(e => e.EventVenueCalendarId)
            .IsUnique()
            .HasFilter("IsHeadliner = 1")
            .HasDatabaseName("UX_ArtistPerformances_OneHeadlinerPerCalendar");

        builder.HasOne(e => e.EventVenueCalendar)
               .WithMany(a => a.Performances)
               .HasForeignKey(e => e.EventVenueCalendarId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Artist>()
            .WithMany()
            .HasForeignKey(e => e.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
