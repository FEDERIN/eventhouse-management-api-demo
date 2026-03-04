using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public abstract class SeatingMapUpsertRequest
{
    [SwaggerSchema(Description = "The unique identifier of the venue to be associated with the seating map.")]
    [Required]
    public Guid VenueId { get; init; }

    /// <summary>Seating map name.</summary>
    /// <example>Main Floor Seating</example>
    [SwaggerSchema(Description = "Seating map name. Must be between 2 and 200 characters.")]
    [Required]
    [MinLength(2)]
    [MaxLength(200)]
    public string Name { get; init; } = null!;

    /// <summary>Indicates whether the seatingMap is currently active.</summary>
    /// <example>true</example>
    [SwaggerSchema(Description = "Indicates whether the seating map is currently active. Defaults to true.")]
    public bool IsActive { get; init; } = true;
}
