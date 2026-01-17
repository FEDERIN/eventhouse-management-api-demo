using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record Artist
{
    /// <summary>Unique identifier of the artist.</summary>
    public required Guid Id { get; init; }

    /// <summary>Public display name of the artist.</summary>
    [SwaggerSchema(
        Description = "Public display name of the artist.",
        Nullable = false)]
    public required string Name { get; init; }

    /// <summary>Category of the artist.</summary>
    [SwaggerSchema(
        Description = "Category that classifies the artist.")]
    public ArtistCategory Category { get; init; }
}
