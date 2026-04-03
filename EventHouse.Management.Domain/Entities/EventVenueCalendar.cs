using EventHouse.Management.Domain.Enums;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class EventVenueCalendar : Entity
{
    public Guid EventVenueId { get; private set; }
    public Guid SeatingMapId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string TimeZoneId { get; private set; } = "UTC";
    public EventVenueCalendarStatus Status { get; private set; }
    public virtual EventVenue? EventVenue { get; private set; }

    private EventVenueCalendar() { }

    public EventVenueCalendar(
        Guid id,
        Guid eventVenueId,
        Guid seatingMapId,
        DateTimeOffset startLocal,
        DateTimeOffset? endLocal,
        string timeZoneId,
        EventVenueCalendarStatus status)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (eventVenueId == Guid.Empty)
            throw new ArgumentException("EventVenueId cannot be empty.", nameof(eventVenueId));
        if (seatingMapId == Guid.Empty)
            throw new ArgumentException("SeatingMapId cannot be empty.", nameof(seatingMapId));


        Id = id;
        EventVenueId = eventVenueId;
        SeatingMapId = seatingMapId;
        TimeZoneId = string.IsNullOrWhiteSpace(timeZoneId) ? "UTC" : timeZoneId;

        var utcStart = startLocal.UtcDateTime;
        var utcEnd = endLocal?.UtcDateTime ?? GetEndOfDayUtc(startLocal);

        ValidateDateRange(utcStart, utcEnd);

        StartDate = utcStart;
        EndDate = utcEnd;
        UpdateStatus(status);
    }

    public void UpdateDates(DateTimeOffset startLocal, DateTimeOffset? endLocal)
    {
        var newStart = startLocal.UtcDateTime;
        var newEnd = endLocal?.UtcDateTime ?? GetEndOfDayUtc(startLocal);

        ValidateDateRange(newStart, newEnd);

        StartDate = newStart;
        EndDate = newEnd;
    }

    public void UpdateStatus(EventVenueCalendarStatus newStatus)
    {
        Status = newStatus;
    }

    private static DateTime GetEndOfDayUtc(DateTimeOffset start)
    {
        return start.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
    }

    private static void ValidateDateRange(DateTime start, DateTime? end)
    {
        if (end.HasValue && end.Value <= start)
        {
            throw new ArgumentException("The end date must be greater than the start date.");
        }
    }
}