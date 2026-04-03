using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;
public class EventVenueCalendarConfiguration : IEntityTypeConfiguration<EventVenueCalendar>
{
    public void Configure(EntityTypeBuilder<EventVenueCalendar> builder)
    {
        builder.ToTable("EventVenueCalendars", t =>
        {
            t.HasCheckConstraint(
                "CK_EventVenueCalendar_EndDate",
                "(EndDate IS NULL OR EndDate >= StartDate)"
            );
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EventVenueId).IsRequired();
        builder.Property(e => e.SeatingMapId).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate);
        builder.Property(e => e.Status).IsRequired();

        builder.HasOne(e=> e.EventVenue)
               .WithMany()
               .HasForeignKey(e => e.EventVenueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SeatingMap>()
               .WithMany()
               .HasForeignKey(e => e.SeatingMapId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.EventVenueId, e.StartDate });

        builder.HasIndex(e => new { e.EventVenueId, e.StartDate, e.EndDate })
            .HasDatabaseName("IX_EventVenueCalendar_Overlap_Search");
    }
}