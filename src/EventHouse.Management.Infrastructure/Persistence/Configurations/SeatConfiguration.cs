using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats", t =>
        {
            t.HasCheckConstraint(
                "CK_Seat_SeatingRowId_NotEmpty",
                "seating_row_id <> '00000000-0000-0000-0000-000000000000'");

            t.HasCheckConstraint(
                "CK_Seat_Number_Positive",
                "number > 0");

            t.HasCheckConstraint(
                "CK_Seat_Label_NotEmpty",
                "TRIM(label) <> ''");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Number)
            .IsRequired();

        builder.Property(e => e.Label)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.HasOne(e => e.SeatingRow)
            .WithMany(e => e.Seats)
            .HasForeignKey(e => e.SeatingRowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new
        {
            e.SeatingRowId,
            e.Number
        })
        .IsUnique()
        .HasDatabaseName("UX_Seats_SeatingRow_Number");
    }
}