using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record Artist
{
    /// <summary>Unique identifier of the artist.</summary>
    public Guid Id { get; init; }

    /// <summary>Display name of the artist.</summary>
    [SwaggerSchema(
        Description = "Public display name of the artist.")]
    public string Name { get; init; } = string.Empty;

    /// <summary>Category of the artist.</summary>
    [SwaggerSchema(
        Description = "Category that classifies the artist.")]
    public ArtistCategory Category { get; init; }
}
