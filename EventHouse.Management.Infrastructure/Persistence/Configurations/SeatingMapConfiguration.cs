using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations
{
    public class SeatingMapConfiguration : IEntityTypeConfiguration<SeatingMap>
    {
        public void Configure(EntityTypeBuilder<SeatingMap> builder)
        {
            builder.ToTable("SeatingMaps", t =>
            {
                t.HasCheckConstraint("CK_SeatingMap_VenueId_NotEmpty", "VenueId <> '00000000-0000-0000-0000-000000000000'");
            });

            builder.HasKey(e => e.Id);

            builder.HasOne<Venue>()
                   .WithMany()
                   .HasForeignKey(e => e.VenueId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Version)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.HasIndex(e => e.VenueId);

            builder.HasIndex(e => new { e.VenueId, e.Name })
                   .IsUnique()
                   .HasDatabaseName("UX_SeatingMap_VenueId_Name");


        }
    }
}
