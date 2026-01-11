using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists", t =>
        {
            t.HasCheckConstraint("CK_Artist_Name_NotEmpty", "TRIM(Name) <> ''");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(e => e.Category)
               .HasConversion<byte>()
               .IsRequired();

        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("IX_Artists_Name");
    }
}
