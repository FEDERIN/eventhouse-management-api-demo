using EventHouse.Management.Domain.ValueObjects;
using EventHouse.ShareKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Domain.Entities;

public class ArtistPerformance : Entity
{
    public Guid EventVenueCalendarId { get; private set; }
    public Guid ArtistId { get; private set; }
    public bool IsHeadliner { get; private set; }

    public DateTime? SetStart { get; private set; }
    public DateTime? SetEnd { get; private set; }

    [NotMapped]
    public TimeRange? TimeRange =>
        SetStart.HasValue && SetEnd.HasValue
            ? TimeRange.Create(SetStart.Value, SetEnd.Value)
            : null;

    [ExcludeFromCodeCoverage]
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
            throw new ArgumentException(null, nameof(eventVenueCalendarId));

        if (artistId == Guid.Empty)
            throw new ArgumentException(null, nameof(artistId));

        EventVenueCalendarId = eventVenueCalendarId;
        ArtistId = artistId;
        IsHeadliner = isHeadliner;

        UpdateTimes(setStartLocal, setEndLocal);
    }

    public void ValidateTimeRange(TimeRange calendarRange)
    {
        if (TimeRange is null)
            return;

        if (!calendarRange.Contains(TimeRange))
        {
            throw new ArgumentException(
                "Performance must be within the calendar boundaries.");
        }
    }

    internal void UpdateHeadlinerStatus(bool isHeadliner)
    {
        IsHeadliner = isHeadliner;
    }

    public void UpdateTimes(
        DateTimeOffset? startLocal,
        DateTimeOffset? endLocal)
    {
        if (startLocal.HasValue != endLocal.HasValue)
            throw new ArgumentException(
                "Start and End must both be provided.");

        if (!startLocal.HasValue)
        {
            SetStart = null;
            SetEnd = null;
            return;
        }

        var range = TimeRange.Create(
            startLocal.Value.UtcDateTime,
            endLocal!.Value.UtcDateTime);

        SetStart = range.Start;
        SetEnd = range.End;
    }
}