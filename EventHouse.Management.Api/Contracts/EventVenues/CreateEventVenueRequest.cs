using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.EventVenues;

public sealed record CreateEventVenueRequest : EventVenueUpsertRequest
{
    /// <summary>The unique identifier of the event to be associated with the event venue.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [SwaggerSchema(Description = "The unique identifier of the event to be associated with the event venue.")]
    [Required]
    public Guid EventId { get; init; }

    /// <summary>The unique identifier of the venue to be associated with the event venue.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [SwaggerSchema(Description = "The unique identifier of the venue to be associated with the event venue.")]
    [Required]
    public Guid VenueId { get; init; }
}
