using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.ArtistPerformances;

public sealed record SwapHeadlinerRequest

{
    /// <summary>
    /// Unique identifier for the performing artist.
    /// </summary>
    [SwaggerSchema(Description = "The unique ID of the artist to be replaced.")]
    [Required]
    public Guid OldArtistId { get; init; }

    ///<summary>
    /// Unique identifier for the performing artist.
    /// </summary>
    [SwaggerSchema(Description = "The unique ID of the artist to be assigned.")]
    [Required]
    public Guid NewArtistId { get; init; }
}