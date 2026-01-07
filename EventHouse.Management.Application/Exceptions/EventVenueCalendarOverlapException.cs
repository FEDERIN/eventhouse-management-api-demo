namespace EventHouse.Management.Application.Exceptions
{
    public class EventVenueCalendarOverlapException : Exception
    {
        public Guid EventId { get; }
        public DateTimeOffset StartDate { get; }
        public DateTimeOffset EndDate { get; }

        public EventVenueCalendarOverlapException(
            Guid eventId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
            : base($"The event '{eventId}' already has a calendar event overlapping with {startDate:u} - {endDate:u}.")
        {
            EventId = eventId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
