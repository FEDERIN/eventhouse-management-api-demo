using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.EventVenueCalendars
{
    public sealed record GetEventVenueCalendarsRequest : SortablePaginationRequest<EventVenueCalendarSortBy>
    {
        [FromQuery(Name = "eventVenueId")]
        public Guid? EventVenueId { get; init; }

        [FromQuery(Name = "seatingMapId")]
        public Guid? SeatingMapId { get; init; }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; init; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; init; }

        [FromQuery(Name = "timeZoneId")]
        public string? TimeZoneId { get; init; }

        [FromQuery(Name = "status")]
        [EnumDataType(typeof(EventVenueCalendarStatus))]
        public EventVenueCalendarStatus? Status { get; init; }
    }
}
