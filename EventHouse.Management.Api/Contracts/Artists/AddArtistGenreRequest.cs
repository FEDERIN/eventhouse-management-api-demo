using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record AddArtistGenreRequest
{
    // <summary>
    // The unique identifier of the genre to be associated with the artist.
    // </summary>
    [SwaggerSchema(Description = "The unique identifier of the genre to be associated with the artist.")]
    [Required]
    public Guid GenreId { get; init; }

    /// <summary>The status of the artist-genre association.</summary>
    /// <example>Active</example>
    [SwaggerSchema(
        Description = "Artist Genre Status. Serialized as string.")]
    [Required]
    [EnumDataType(typeof(ArtistGenreStatus))]
    public ArtistGenreStatus Status { get; init; } = ArtistGenreStatus.Active;

    // <summary>
    // Indicates if this genre is the primary genre for the artist.
    // </summary>
    [SwaggerSchema(Description = "Indicates if this genre is the primary genre for the artist.")]
    [Required]
    public bool IsPrimary { get; init; } = false;
}
