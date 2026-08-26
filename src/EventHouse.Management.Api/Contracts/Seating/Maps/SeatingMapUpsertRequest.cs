using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Seating.Maps;

public abstract class SeatingMapUpsertRequest
{
    /// <summary>Seating map name.</summary>
    [SwaggerSchema(Description = "Seating map name. Must be between 2 and 200 characters.")]
    [Required]
    [MinLength(2)]
    [MaxLength(200)]
    public string Name { get; init; } = null!;
}
