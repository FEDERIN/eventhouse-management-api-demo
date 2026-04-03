using EventHouse.Management.Api.Contracts.EventVenues;

namespace EventHouse.Management.Api.Tests.Factories;

internal static class EventVenueFactory
{
    public static CreateEventVenueRequest CreateRequest(Guid? eventId = null, Guid ? venueId = null, EventVenueStatus status = EventVenueStatus.Active)
    {
        return new CreateEventVenueRequest
        {
            EventId = eventId ?? Guid.NewGuid(),
            VenueId = venueId ?? Guid.NewGuid(),
            Status = status
        };
    }
}
