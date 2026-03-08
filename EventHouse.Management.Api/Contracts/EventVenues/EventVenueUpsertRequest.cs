using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.EventVenues;

public abstract record EventVenueUpsertRequest
{
    /// <summary>The status of the event venue.</summary>
    /// <example>Active</example>
    [Required]
    [SwaggerSchema(Description = "The status of the event venue.")]
    public EventVenueStatus Status { get; init; }
}
