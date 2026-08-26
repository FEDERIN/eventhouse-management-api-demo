using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class SeatingSectionConfiguration
    : IEntityTypeConfiguration<SeatingSection>
{
    public void Configure(EntityTypeBuilder<SeatingSection> builder)
    {
        builder.ToTable("SeatingSections", t =>
        {
            t.HasCheckConstraint(
                "CK_SeatingSection_SeatingMapId_NotEmpty",
                "seating_map_id <> '00000000-0000-0000-0000-000000000000'");

            t.HasCheckConstraint(
                "CK_SeatingSection_Name_NotEmpty",
                "TRIM(name) <> ''");

            t.HasCheckConstraint(
                "CK_SeatingSection_Capacity_Positive",
                "capacity > 0");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SeatingMapId)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.IsNumbered)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.HasOne(e => e.SeatingMap)
            .WithMany(e => e.Sections)
            .HasForeignKey(e => e.SeatingMapId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Rows)
            .WithOne(e => e.SeatingSection)
            .HasForeignKey(e => e.SeatingSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new
        {
            e.SeatingMapId,
            e.Name
        })
        .IsUnique()
        .HasDatabaseName("UX_SeatingSections_SeatingMap_Name");
    }
}