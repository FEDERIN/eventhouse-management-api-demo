
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public sealed class ArtistDto
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public ArtistCategoryDto Category { get; init; }
        public IReadOnlyList<ArtistGenreDto> Genres = [];
    }
}
