using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Region)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.CountryCode)
            .IsRequired()
            .HasMaxLength(2)
            .IsFixedLength();

        builder.Property(v => v.Latitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(v => v.Longitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(v => v.TimeZoneId)
            .HasMaxLength(64);

        builder.Property(v => v.Capacity);

        builder.Property(v => v.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(v => v.Name)
               .IsUnique()
               .HasDatabaseName("UX_Venues_Name");

        builder.HasIndex(v => new { v.CountryCode, v.City });
    }
}
