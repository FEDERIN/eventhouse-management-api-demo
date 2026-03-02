using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", t =>
        {
            t.HasCheckConstraint("CK_Genre_Name_NotEmpty", "TRIM(Name) <> ''");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasIndex(e => e.Name)
               .IsUnique()
               .HasDatabaseName("UX_Genres_Name");
    }
}
