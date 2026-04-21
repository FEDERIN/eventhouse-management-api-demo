using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.EventVenueCalendars;

public sealed record CreateEventVenueCalendarRequest : EventVenueCalendarUpsertRequest
{
    /// <summary>The unique identifier of the physical venue where the event will take place.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [SwaggerSchema(Description = "The unique identifier of the physical venue where the event will take place.")]
    [Required]
    public Guid EventVenueId { get; init; }

    /// <summary>The unique identifier of the specific seating configuration to be used for this event.</summary>
    /// <example>1b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed</example>
    [SwaggerSchema(Description = "The unique identifier of the specific seating configuration to be used for this event.")]
    [Required]
    public Guid SeatingMapId { get; init; }

    /// <summary>Event time zone (IANA). If not provided, UTC is used.</summary>
    /// <example>Example: Europe/Malta</example>
    [SwaggerSchema(Description = "The IANA time zone identifier for the event (e.g., 'Europe/Malta'). Defaults to 'UTC' if not specified.")]
    [Required, MinLength(1)]
    public string TimeZoneId { get; init; } = "America/New_York";
}