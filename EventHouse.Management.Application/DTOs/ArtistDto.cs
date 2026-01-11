
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    /// <summary>
    /// Artist information returned by the management API.
    /// </summary>
    public sealed class ArtistDto
    {
        /// <summary>Unique identifier of the artist.</summary>
        public Guid Id { get; init; }

        /// <summary>Display name of the artist.</summary>
        public required string Name { get; init; }

        /// <summary>Category of the artist (Singer, Band, DJ, etc.).</summary>
        public ArtistCategory Category { get; init; }
    }
}
