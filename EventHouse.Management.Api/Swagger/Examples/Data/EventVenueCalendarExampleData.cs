using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class EventVenueCalendarExampleData
{
    private static readonly Guid SeatingMapId = ExampleConstants.SeatingMapId;
    private static readonly Guid EventVenueId = ExampleConstants.EventVenueId;
    private static readonly string TimeZoneId = ExampleConstants.TimeZoneId;

    internal static CreateEventVenueCalendarRequest Create() => new()
    {
        EventVenueId = EventVenueId,
        SeatingMapId = SeatingMapId,
        TimeZoneId = TimeZoneId,
        StartDate = new DateTimeOffset(2026, 12, 6, 20, 0, 0, TimeSpan.FromHours(1)),
        EndDate = new DateTimeOffset(2026, 12, 6, 23, 0, 0, TimeSpan.FromHours(1)),
        Status = EventVenueCalendarStatus.Draft
    };

    internal static UpdateEventVenueCalendarRequest Update() => new()
    {
        StartDate = new DateTimeOffset(2026, 12, 10, 18, 0, 0, TimeSpan.FromHours(1)),
        EndDate = new DateTimeOffset(2026, 12, 10, 22, 0, 0, TimeSpan.FromHours(1)),
        Status = EventVenueCalendarStatus.Published
    };

    internal static EventVenueCalendarResponse Result() => new()
    {
        Id = Guid.NewGuid(),
        EventVenueId = EventVenueId,
        SeatingMapId = SeatingMapId,
        StartDate = new DateTimeOffset(2026, 12, 6, 20, 0, 0, TimeSpan.FromHours(1)),
        EndDate = new DateTimeOffset(2026, 12, 6, 23, 0, 0, TimeSpan.FromHours(1)),
        TimeZoneId = TimeZoneId,
        Status = EventVenueCalendarStatus.Published
    };

    internal static GetEventVenueCalendarsRequest Get() => new()
    {
        EventVenueId = EventVenueId,
        SeatingMapId = SeatingMapId,
        StartDate = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        EndDate = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc),
        TimeZoneId = TimeZoneId,
        Status = EventVenueCalendarStatus.Published,

        Page = 1,
        PageSize = 20,
    };
}