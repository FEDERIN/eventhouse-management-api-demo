using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Artists;
using EventHouse.Management.Domain.Exceptions.Calendars;
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

    private readonly List<ArtistPerformance> _performances = [];
    public virtual IReadOnlyCollection<ArtistPerformance> Performances => _performances.AsReadOnly();

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
        if (newStatus == EventVenueCalendarStatus.Published)
        {
            var incomplete = _performances.FirstOrDefault(p => p.SetStart == null || p.SetEnd == null);
            if (incomplete != null)
                throw new PerformanceDatesRequiredException(Id, incomplete.ArtistId);
        }

        Status = newStatus;
    }

    public AddCalendarOutcome AddPerformance(Guid artistId, bool isHeadliner, DateTimeOffset? startLocal, DateTimeOffset? endLocal)
    {
        if (_performances.Any(p => p.ArtistId == artistId))
        {
            return AddCalendarOutcome.NoChange;
        }

        if (isHeadliner && _performances.Any(p => p.IsHeadliner))
        {
            throw new DuplicateHeadlinerException(Id);
        }

        if (Status == EventVenueCalendarStatus.Published && (!startLocal.HasValue || !endLocal.HasValue))
        {
            throw new PerformanceDatesRequiredException(Id, artistId);
        }

        if (startLocal.HasValue && endLocal.HasValue)
        {
            var start = startLocal.Value.UtcDateTime;
            var end = endLocal.Value.UtcDateTime;

            if (_performances.Any(p => start < p.SetEnd && p.SetStart < end))
            {
                throw new StageOverlapException(Id, start, end);
            }
        }

        var performance = new ArtistPerformance(
            Id,
            artistId,
            isHeadliner,
            startLocal,
            endLocal);

        performance.ValidateTimeRange(StartDate, EndDate ?? DateTime.MaxValue);

        _performances.Add(performance);

        return AddCalendarOutcome.Added;
    }

    public void UpdatePerformance(Guid artistId, DateTimeOffset? startLocal, DateTimeOffset? endLocal)
    {
        var performance = _performances.FirstOrDefault(p => p.ArtistId == artistId)
            ?? throw new NotFoundException("ArtistPerformance", artistId);

        if (startLocal.HasValue && endLocal.HasValue)
        {
            var start = startLocal.Value.UtcDateTime;
            var end = endLocal.Value.UtcDateTime;

            if (_performances.Any(p => p.ArtistId != artistId &&  start < p.SetEnd && p.SetStart < end))
            {
                throw new StageOverlapException(Id, start, end);
            }
        }

        performance.UpdateTimes(startLocal, endLocal);
        performance.ValidateTimeRange(StartDate, EndDate);

        if (Status == EventVenueCalendarStatus.Published && (performance.SetStart == null || performance.SetEnd == null))
        {
            throw new PerformanceDatesRequiredException(Id, artistId);
        }
    }

    public void SwapHeadliner(Guid currentHeadlinerArtistId, Guid newHeadlinerArtistId)
    {
        var oldHeadliner = _performances.FirstOrDefault(p => p.ArtistId == currentHeadlinerArtistId && p.IsHeadliner)
                ?? throw new ArtistIsNotHeadlinerException(Id, currentHeadlinerArtistId);

        var newHeadliner = _performances.FirstOrDefault(p => p.ArtistId == newHeadlinerArtistId)
                ?? throw new NotFoundException("ArtistPerformance", newHeadlinerArtistId);

        oldHeadliner.UpdateHeadlinerStatus(false);
        newHeadliner.UpdateHeadlinerStatus(true);

        if (Status == EventVenueCalendarStatus.Published)
        {
            if (newHeadliner.SetStart == null || newHeadliner.SetEnd == null)
            {
                throw new PerformanceDatesRequiredException(Id, newHeadlinerArtistId);
            }
        }
    }

    public void RemovePerformance(Guid artistId)
    {
        var existing = _performances.FirstOrDefault(p => p.ArtistId == artistId);

        if (existing is null)
            return;

        if (Status == EventVenueCalendarStatus.Published && existing.IsHeadliner)
        {
            throw new CannotRemovePublishedHeadlinerException(Id, artistId);
        }

        _performances.Remove(existing);
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

public enum AddCalendarOutcome
{
    Added,
    NoChange
}