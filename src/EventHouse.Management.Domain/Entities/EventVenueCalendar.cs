using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Calendars;
using EventHouse.Management.Domain.ValueObjects;
using EventHouse.ShareKernel.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Domain.Entities;

public class EventVenueCalendar : Entity
{
    public Guid EventVenueId { get; private set; }
    public Guid SeatingMapId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TimeZoneId TimeZoneId { get; private set; } = TimeZoneId.Create(null);
    public EventVenueCalendarStatus Status { get; private set; }

    private TimeRange CalendarRange =>
        TimeRange.Create(StartDate, EndDate);

    [ExcludeFromCodeCoverage]
    public virtual EventVenue? EventVenue { get; private set; }

    private readonly List<ArtistPerformance> _performances = [];

    public virtual IReadOnlyCollection<ArtistPerformance> Performances =>
        _performances.AsReadOnly();

    private EventVenueCalendar() { }

    public EventVenueCalendar(
        Guid id,
        Guid eventVenueId,
        Guid seatingMapId,
        DateTimeOffset startLocal,
        DateTimeOffset endLocal,
        string timeZoneId)
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

        SetTimeZone(timeZoneId);

        var range = TimeRange.Create(
            startLocal.UtcDateTime,
            endLocal.UtcDateTime);

        StartDate = range.Start;
        EndDate = range.End;

        Status = EventVenueCalendarStatus.Draft;
    }

    public void UpdateDates(
        DateTimeOffset startLocal,
        DateTimeOffset endLocal)
    {
        var range = TimeRange.Create(
            startLocal.UtcDateTime,
            endLocal.UtcDateTime);

        StartDate = range.Start;
        EndDate = range.End;
    }

    public void ChangeStatus(EventVenueCalendarStatus newStatus)
    {
        if (!CanTransitionTo(newStatus))
            throw new InvalidCalendarStatusTransitionException(
                Status,
                newStatus);

        if (newStatus == EventVenueCalendarStatus.Published)
            EnsureCanPublish();

        Status = newStatus;
    }

    private void SetTimeZone(string? timeZoneId)
    {
        TimeZoneId = TimeZoneId.Create(timeZoneId);
    }

    public AddCalendarOutcome AddPerformance(
        Guid artistId,
        bool isHeadliner,
        DateTimeOffset? startLocal,
        DateTimeOffset? endLocal)
    {
        if (_performances.Any(p => p.ArtistId == artistId))
            return AddCalendarOutcome.NoChange;

        if (isHeadliner && _performances.Any(p => p.IsHeadliner))
            throw new DuplicateHeadlinerException(Id);

        EnsurePerformanceHasDatesIfPublished(
            artistId,
            startLocal,
            endLocal);

        TimeRange? performanceRange = null;

        if (startLocal.HasValue)
        {
            performanceRange = TimeRange.Create(
                startLocal.Value.UtcDateTime,
                endLocal!.Value.UtcDateTime);

            if (_performances.Any(p =>
                    p.TimeRange is not null &&
                    p.TimeRange.Overlaps(performanceRange)))
            {
                throw new StageOverlapException(
                    Id,
                    performanceRange.Start,
                    performanceRange.End);
            }
        }

        var performance = new ArtistPerformance(
            Id,
            artistId,
            isHeadliner,
            startLocal,
            endLocal);

        performance.ValidateTimeRange(CalendarRange);

        _performances.Add(performance);

        return AddCalendarOutcome.Added;
    }

    public void UpdatePerformance(
        Guid artistId,
        DateTimeOffset? startLocal,
        DateTimeOffset? endLocal)
    {
        var performance = _performances.FirstOrDefault(
            p => p.ArtistId == artistId)
            ?? throw new NotAssociatedException(
                "EventVenueCalendar",
                "ArtistPerformance",
                Id,
                artistId);

        if (startLocal.HasValue)
        {
            var newRange = TimeRange.Create(
                startLocal.Value.UtcDateTime,
                endLocal!.Value.UtcDateTime);

            if (_performances.Any(p =>
                    p.ArtistId != artistId &&
                    p.TimeRange is not null &&
                    p.TimeRange.Overlaps(newRange)))
            {
                throw new StageOverlapException(
                    Id,
                    newRange.Start,
                    newRange.End);
            }
        }

        performance.UpdateTimes(
            startLocal,
            endLocal);

        performance.ValidateTimeRange(CalendarRange);

        EnsurePerformanceHasDatesIfPublished(
            artistId,
            performance.SetStart,
            performance.SetEnd);
    }

    public void ValidateHeadlinerSwap(
        Guid currentHeadlinerArtistId,
        Guid newHeadlinerArtistId)
    {
        var oldHeadlinerExists = _performances.Any(
            p => p.ArtistId == currentHeadlinerArtistId &&
                 p.IsHeadliner);

        if (!oldHeadlinerExists)
            throw new ArtistIsNotHeadlinerException(
                Id,
                currentHeadlinerArtistId);

        var newHeadlinerExists = _performances.Any(
            p => p.ArtistId == newHeadlinerArtistId);

        if (!newHeadlinerExists)
            throw new NotAssociatedException(
                "EventVenueCalendar",
                "ArtistPerformance",
                Id,
                newHeadlinerArtistId);
    }

    public void RemovePerformance(Guid artistId)
    {
        var existing = _performances.FirstOrDefault(
            p => p.ArtistId == artistId);

        if (existing is null)
            return;

        if (Status == EventVenueCalendarStatus.Published &&
            existing.IsHeadliner)
        {
            throw new CannotRemovePublishedHeadlinerException(
                Id,
                artistId);
        }

        _performances.Remove(existing);
    }

    private void EnsurePerformanceHasDatesIfPublished(
        Guid artistId,
        DateTimeOffset? start,
        DateTimeOffset? end)
    {
        if (Status == EventVenueCalendarStatus.Published &&
            (!start.HasValue || !end.HasValue))
        {
            throw new PerformanceDatesRequiredException(
                Id,
                artistId);
        }
    }

    private bool CanTransitionTo(EventVenueCalendarStatus newStatus)
    {
        return (Status, newStatus) switch
        {
            (EventVenueCalendarStatus.Draft,
                EventVenueCalendarStatus.Published) => true,

            (EventVenueCalendarStatus.Draft,
                EventVenueCalendarStatus.Cancelled) => true,

            (EventVenueCalendarStatus.Published,
                EventVenueCalendarStatus.Cancelled) => true,

            _ => false
        };
    }
    private void EnsureCanPublish()
    {
        var incomplete = _performances
            .FirstOrDefault(p => p.TimeRange is null);

        if (incomplete is not null)
            throw new PerformanceDatesRequiredException(
                Id,
                incomplete.ArtistId);
    }
}

public enum AddCalendarOutcome
{
    Added,
    NoChange
}