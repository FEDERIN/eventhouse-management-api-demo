using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public sealed class UpdateSeatingMapRequest : SeatingMapUpsertRequest
{
    /// <summary>The current version of the seating map. Must match the version on the server for the update to succeed.</summary>
    /// <example>2</example>
    [SwaggerSchema(Description = "The current version of the seating map. Must match the version on the server for the update to succeed.")]
    [Required]
    public int Version { get; init; }
}