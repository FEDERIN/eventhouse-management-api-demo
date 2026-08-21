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
}
