using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record UpdateArtistGenreStatusRequest
{
    /// <summary>The status of the artist-genre association.</summary>
    /// <example>Active</example>
    [SwaggerSchema(
        Description = "Artist Genre Status. Serialized as string.")]
    [Required]
    [EnumDataType(typeof(ArtistGenreStatus))]
    public ArtistGenreStatus Status { get; init; }
}
