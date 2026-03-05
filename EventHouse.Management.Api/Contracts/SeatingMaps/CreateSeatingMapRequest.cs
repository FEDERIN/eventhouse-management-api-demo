
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.SeatingMaps;

public sealed class CreateSeatingMapRequest : SeatingMapUpsertRequest{

    /// <summary>The unique identifier of the venue to be associated with the seating map.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [SwaggerSchema(Description = "The unique identifier of the venue to be associated with the seating map.")]
    [Required]
    public Guid VenueId { get; init; }
}