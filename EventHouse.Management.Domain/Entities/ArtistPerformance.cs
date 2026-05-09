using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class ArtistPerformance : Entity
{
    public Guid EventVenueCalendarId { get; private set; }
    public Guid ArtistId { get; private set; }
    public bool IsHeadliner { get; private set; }
    public DateTime? SetStart { get; private set; } // Stored in UTC
    public DateTime? SetEnd { get; private set; }   // Stored in UTC
    public virtual EventVenueCalendar? EventVenueCalendar { get; private set; }

    private ArtistPerformance() { }

    public ArtistPerformance(
        Guid eventVenueCalendarId,
        Guid artistId,
        bool isHeadliner,
        DateTimeOffset? setStartLocal,
        DateTimeOffset? setEndLocal)
    {
        if (eventVenueCalendarId == Guid.Empty) 
            throw new ArgumentException("EventVenueCalendarId is required.");

        if (artistId == Guid.Empty) 
            throw new ArgumentException("ArtistId is required.");

        //Id = id;
        EventVenueCalendarId = eventVenueCalendarId;
        ArtistId = artistId;
        IsHeadliner = isHeadliner;

        // Normalize to UTC for persistence
        SetStart = setStartLocal?.UtcDateTime;
        SetEnd = setEndLocal?.UtcDateTime;
    }

    /// <summary>
    /// Validates that the performance timing fits within the specified calendar boundaries.
    /// Comparisons are made in UTC to ensure accuracy.
    /// </summary>
    public void ValidateTimeRange(DateTime calendarStartUtc, DateTime? calendarEndUtc)
    {
        DateTime? start = SetStart.HasValue
            ? Trim(SetStart.Value)
            : null;

        DateTime? end = SetEnd.HasValue
            ? Trim(SetEnd.Value)
            : null;

        DateTime calStart = Trim(calendarStartUtc);

        DateTime? calEnd = calendarEndUtc.HasValue
            ? Trim(calendarEndUtc.Value)
            : null;

        if (start.HasValue && start < calStart)
            throw new ArgumentException("Performance cannot start before the calendar slot begins.");

        if (end.HasValue && calEnd.HasValue && end > calEnd)
            throw new ArgumentException("Performance cannot end after the calendar slot expires.");

        if (start.HasValue && end.HasValue && start >= end)
            throw new ArgumentException("Start time must be earlier than end time.");
    }

    private static DateTime Trim(DateTime date)
    {
        return new DateTime(date.Ticks - (date.Ticks % TimeSpan.TicksPerSecond), date.Kind);
    }

    internal void UpdateHeadlinerStatus(bool isHeadliner)
    {
        IsHeadliner = isHeadliner;
    }

    public void UpdateTimes(DateTimeOffset? setStartLocal, DateTimeOffset? setEndLocal)
    {
        SetStart = setStartLocal?.UtcDateTime;
        SetEnd = setEndLocal?.UtcDateTime;
    }
}