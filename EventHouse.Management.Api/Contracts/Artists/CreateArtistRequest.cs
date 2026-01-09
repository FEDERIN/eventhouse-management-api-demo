using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record CreateArtistRequest
{
    /// <summary>
    /// Public display name of the artist.
    /// </summary>
    /// <example>The Rolling Stones</example>
    [SwaggerSchema(
        Description = "Must be between 2 and 200 characters.")]
    [Required]
    [MinLength(2)]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Category that classifies the artist.
    /// </summary>
    [SwaggerSchema(
        Description = "Artist category. Serialized as string.")]
    [Required]
    [EnumDataType(typeof(ArtistCategory))]
    public ArtistCategory Category { get; init; }
}
