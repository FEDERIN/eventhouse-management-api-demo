using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class EventVenueExampleData
{
    private static readonly Guid EventVenueId = ExampleConstants.EventVenueId;
    private static readonly Guid EventId = ExampleConstants.EventId;
    private static readonly Guid VenueId = ExampleConstants.VenueId;
    private static readonly string EventName = ExampleConstants.EventName;
    private static readonly string VenueName = ExampleConstants.VenueName;
    private static readonly EventVenueStatus Status = EventVenueStatus.Active;

    internal static CreateEventVenueRequest Create() => new()
    {
        EventId = EventId,
        VenueId = VenueId,
        Status = Status
    };

    internal static UpdateEventVenueStatusRequest Update() => new() { 
        Status = Status
    };

    internal static EventVenueResponse Result() => new()
    {
        Id = EventVenueId,
        EventId = EventId,
        VenueId = VenueId,
        Status = Status,
        EventName = EventName,
        VenueName = VenueName
    };

    internal static GetEventVenuesRequest Get() => new()
    {
        EventId = EventId,
        VenueId = VenueId,
        Status = Status,
        Page = 1,
        PageSize = 20,
        SortBy = EventVenueSortBy.Status,
        SortDirection = SortDirection.Asc
    };
}
