
namespace EventHouse.Management.Api.Contracts.EventVenueCalendars;

public sealed class EventVenueCalendarResponse
{
    public Guid Id { get; init; }

    public Guid EventVenueId { get; init; }

    public Guid SeatingMapId { get; init; }

    public DateTimeOffset StartDate { get; init; }

    public DateTimeOffset? EndDate { get; init; }

    public string TimeZoneId { get; init; } = default!;

    public EventVenueCalendarStatus Status { get; init; }
}
