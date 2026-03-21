using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Infrastructure.Tests.Factories;

public static class TestEntityFactory
{
    public static Event CreateEvent(Guid id, string name = "Test Event", EventScope scope = EventScope.Local)
    {

        return new Event(id, $"{name} {Guid.NewGuid().ToString()[..4]}", "Description", scope);
    }

    public static Venue CreateVenue(Guid id, string name = "Test Venue", string city = "Test City", bool isActive = true)
    {
        return new Venue(
            id, $"{name} {Guid.NewGuid().ToString()[..4]}", "Test Address", city, "Test Region", "US", 0, 0, "UTC", 100, isActive);
    }

    public static EventVenue CreateEventVenue(Guid id, Guid eventId, Guid venueId, EventVenueStatus status = EventVenueStatus.Active)
    {
        return new EventVenue(id, eventId, venueId, status);
    }

    public static SeatingMap CreateSeatingMap(Guid id, Guid venueId, string name = "Main Map", int version = 1, bool isActive = true)
    {
        return new SeatingMap(id, venueId, name, version, isActive);
    }

    public static EventVenueCalendar CreateEventVenueCalendar(
        Guid id,
        Guid eventVenueId,
        Guid seatingMapId,
        DateTimeOffset? startLocal = null,
        DateTimeOffset? endLocal = null,
        string timeZoneId = "UTC",
        EventVenueCalendarStatus status = EventVenueCalendarStatus.Published)
    {
        return new EventVenueCalendar(
            id,
            eventVenueId,
            seatingMapId,
            startLocal ?? DateTimeOffset.UtcNow,
            endLocal,
            timeZoneId,
            status);
    }
}