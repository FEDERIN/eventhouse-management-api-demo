using EventHouse.Management.Domain.Enums;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities
{
    public sealed class ArtistGenre : Entity
    {
        public Guid ArtistId { get; private set; }
        public Guid GenreId { get; private set; }

        public ArtistGenreStatus  Status { get; private set; }

        public bool IsPrimary { get; private set; }

        private ArtistGenre() { }

        internal ArtistGenre(Guid artistId, Guid genreId, ArtistGenreStatus status, bool isPrimary)
        {
            if (artistId == Guid.Empty)
                throw new ArgumentException("ArtistId cannot be empty.", nameof(artistId));

            if (genreId == Guid.Empty)
                throw new ArgumentException("GenreId cannot be empty.", nameof(genreId));

            if (!Enum.IsDefined(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid ArtistGenreStatus value.");

            ArtistId = artistId;
            GenreId = genreId;
            Status = status;
            IsPrimary = isPrimary;
        }

        internal void MarkAsPrimary()
        {
            IsPrimary = true;
        }

        internal void MarkAsSecondary()
        {
            IsPrimary = false;
        }

        internal void SetStatus(ArtistGenreStatus status)
        {
            if (!Enum.IsDefined(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid ArtistGenreStatus value.");

            Status = status;
        }
    }
}
