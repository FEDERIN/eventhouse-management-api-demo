using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Seating.Maps;

public sealed class CreateSeatingMapRequest : SeatingMapUpsertRequest{

    /// <summary>The unique identifier of the venue to be associated with the seating map.</summary>
    [SwaggerSchema(Description = "The unique identifier of the venue to be associated with the seating map.")]
    [Required]
    public Guid VenueId { get; init; }

    [SwaggerSchema(Description = "The version of the Seating Map")] 
    [Required]
    public int Version { get; init; }
}