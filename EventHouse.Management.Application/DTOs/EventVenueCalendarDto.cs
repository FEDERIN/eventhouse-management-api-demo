
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public class EventVenueCalendarDto
    {
        public Guid Id { get; set; }
        public Guid EventVenueId { get; set; }
        public Guid SeatingMapId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string TimeZoneId { get; set; } = "UTC";
        public EventVenueCalendarStatus Status { get; set; }
    }
}
