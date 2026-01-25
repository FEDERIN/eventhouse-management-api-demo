
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public sealed class ArtistGenreDto
    {
        public Guid GenreId { get; set; }
        public bool IsPrimary { get; set; }
        public ArtistGenreStatus Status { get; set; }
    }
}
