using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

public class EventVenueConfiguration : IEntityTypeConfiguration<EventVenue>
{
    public void Configure(EntityTypeBuilder<EventVenue> builder)
    {
        builder.ToTable("EventVenues", t =>
        {
            t.HasCheckConstraint("CK_EventVenue_EventId_NotEmpty", "EventId <> '00000000-0000-0000-0000-000000000000'");
            t.HasCheckConstraint("CK_EventVenue_VenueId_NotEmpty", "VenueId <> '00000000-0000-0000-0000-000000000000'");
        });

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.EventId, e.VenueId })
               .IsUnique()
               .HasDatabaseName("UX_EventVenues_Event_Venue");

        builder.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Status)
               .IsRequired();

        builder.HasMany<EventVenueCalendar>()
               .WithOne(c => c.EventVenue)
               .HasForeignKey(c => c.EventVenueId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}