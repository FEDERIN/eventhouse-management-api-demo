using EventHouse.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class ArtistGenreConfiguration : IEntityTypeConfiguration<ArtistGenre>
{
    public void Configure(EntityTypeBuilder<ArtistGenre> builder)
    {
        builder.ToTable("ArtistGenres", t =>
        {
            t.HasCheckConstraint("CK_ArtistGenre_ArtistId_NotEmpty", "ArtistId <> '00000000-0000-0000-0000-000000000000'");
            t.HasCheckConstraint("CK_ArtistGenre_GenreId_NotEmpty", "GenreId <> '00000000-0000-0000-0000-000000000000'");
        });

        builder.HasKey(e => e.Id);

        builder.HasOne<Artist>()
               .WithMany(a => a.Genres)
               .HasForeignKey(e => e.ArtistId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Genre>()
               .WithMany()
               .HasForeignKey(e => e.GenreId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.ArtistId, e.GenreId })
               .IsUnique()
               .HasDatabaseName("UX_ArtistGenres_Artist_Genre");

        builder.Property(e => e.Status)
               .IsRequired();

        builder.Property(e => e.IsPrimary)
               .IsRequired();

        builder.HasIndex(e => e.ArtistId)
               .IsUnique()
               .HasFilter("IsPrimary = 1")
               .HasDatabaseName("UX_ArtistGenres_Artist_Primary");
    }
}
