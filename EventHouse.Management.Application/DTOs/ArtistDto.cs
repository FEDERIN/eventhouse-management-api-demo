
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public sealed class ArtistDto
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public ArtistCategory Category { get; init; }
    }
}
