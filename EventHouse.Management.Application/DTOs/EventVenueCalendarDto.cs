
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs
{
    public class EventVenueCalendarDto
    {
        public Guid Id { get; set; }
        public Guid EventVenueId { get; set; }
        public Guid SeatingMapId { get; set; }
        /// <summary>
        /// Fecha y hora de inicio en ISO-8601 con offset. Ejemplo: 2025-12-06T22:00:00+01:00
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Fecha y hora de fin en ISO-8601 con offset.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Zona horaria del evento en formato IANA (ej: "Europe/Malta")
        /// </summary>
        public string TimeZoneId { get; set; } = "UTC";

        public EventVenueCalendarStatus Status { get; set; }
    }
}
