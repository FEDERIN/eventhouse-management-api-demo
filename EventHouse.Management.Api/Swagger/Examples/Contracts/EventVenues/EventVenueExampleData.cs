using EventHouse.Management.Api.Contracts.EventVenues;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;

[ExcludeFromCodeCoverage]
internal static class EventVenueExampleData
{
    internal static CreateEventVenueRequest Create() => new()
    {
        EventId = new Guid("d290f1ee-6c54-4b01-90e6-d701748f0852"),
        VenueId = new Guid("d290f1ee-6c54-4b01-90e6-d701748f0853"),
        Status = EventVenueStatus.Active
    };

    internal static UpdateEventVenueStatusRequest Update() => new() { 
        Status = EventVenueStatus.Inactive 
    };

    internal static EventVenueResponse Result() => new()
    {
        Id = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
        EventId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852"),
        VenueId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0853"),
        Status = EventVenueStatus.Active,
        EventName = "Sample Event",
        VenueName = "Sample Venue"
    };

    internal static GetEventVenuesRequest Get() => new()
    {
        EventId = new Guid("d290f1ee-6c54-4b01-90e6-d701748f0852"),
        VenueId = new Guid("d290f1ee-6c54-4b01-90e6-d701748f0853"),
        Status = EventVenueStatus.Active,
    };

}
