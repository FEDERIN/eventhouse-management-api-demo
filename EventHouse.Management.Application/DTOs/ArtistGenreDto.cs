
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public class ArtistGenreDto
    {
        public Guid GenreId { get; set; }
        public bool IsPrimary { get; set; }
        public ArtistGenreStatus Status { get; set; }
    }
}
