using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.EventVenueCalendars;

public abstract record EventVenueCalendarUpsertRequest
{
    ///<summary>Start date and time in ISO-8601 format with offset.</summary>
    ///<example>2025-12-06T22:00:00+01:00</example>
    [SwaggerSchema(Description = "Start date and time in ISO-8601 format with offset (e.g. 2025-12-06T22:00:00+01:00).")]
    [Required]
    public DateTimeOffset StartDate { get; set; }

    ///<summary>End date and time in ISO-8601 format with offset.</summary>
    ///<example>2025-12-07T00:00:00+01:00</example>
    [SwaggerSchema(Description = "End date and time in ISO-8601 format with offset (e.g. 2025-12-07T00:00:00+01:00).")]
    public DateTimeOffset? EndDate { get; set; }

    ///<summary>The status of the event venue calendar.</summary>
    ///<example>Draft</example>
    [SwaggerSchema(Description = "The status of the event venue calendar.")]
    [Required]
    public EventVenueCalendarStatus Status { get; set; }
}
