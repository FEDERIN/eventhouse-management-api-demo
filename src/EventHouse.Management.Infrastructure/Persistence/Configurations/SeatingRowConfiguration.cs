using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class SeatingRowConfiguration
    : IEntityTypeConfiguration<SeatingRow>
{
    public void Configure(EntityTypeBuilder<SeatingRow> builder)
    {
        builder.ToTable("SeatingRows", t =>
        {
            t.HasCheckConstraint(
                "CK_SeatingRow_SeatingSectionId_NotEmpty",
                "seating_section_id <> '00000000-0000-0000-0000-000000000000'");

            t.HasCheckConstraint(
                "CK_SeatingRow_Number_Positive",
                "number > 0");

            t.HasCheckConstraint(
                "CK_SeatingRow_Label_NotEmpty",
                "TRIM(label) <> ''");
        });

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SeatingSectionId)
            .IsRequired();

        builder.Property(e => e.Number)
            .IsRequired();

        builder.Property(e => e.Label)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.HasOne(e => e.SeatingSection)
            .WithMany(e => e.Rows)
            .HasForeignKey(e => e.SeatingSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new
        {
            e.SeatingSectionId,
            e.Number
        })
        .IsUnique()
        .HasDatabaseName("UX_SeatingRows_SeatingSection_Number");
    }
}