using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.TestUtils.Factories;

public static class TestEntityFactory
{
    /// <summary>
    /// Creates a Genre entity with a randomized name to avoid collisions.
    /// </summary>
    public static Genre CreateGenre(Guid? id = null, string name = "Rock")
    {
        var genreId = id ?? Guid.NewGuid();
        var uniqueName = $"{name} {Guid.NewGuid().ToString()[..4]}";

        return new Genre(genreId, uniqueName);
    }

    /// <summary>
    /// Creates a Venue entity that satisfies all domain validations.
    /// </summary>
    public static Venue CreateVenue(
        Guid? id = null,
        string name = "Madison Square Garden",
        string address = "4 Pennsylvania Plaza",
        string city = "New York",
        string region = "NY",
        string countryCode = "US",
        decimal? latitude = 40.7505m,
        decimal? longitude = -73.9934m,
        string? timeZoneId = "Eastern Standard Time",
        int? capacity = 20000,
        bool isActive = true)
    {
        return new Venue(
            id ?? Guid.NewGuid(),
            $"{name} {Guid.NewGuid().ToString()[..4]}",
            address,
            city,
            region,
            countryCode,
            latitude,
            longitude,
            timeZoneId,
            capacity,
            isActive);
    }

    public static Event CreateEvent(Guid id, string name = "Test Event", EventScope scope = EventScope.Local)
    {
        return new Event(id, $"{name} {Guid.NewGuid().ToString()[..4]}", "Description", scope);
    }

    public static SeatingMap CreateSeatingMap(Guid id, Guid venueId, string name = "Main Map", int version = 1, bool isActive = true)
    {
        return new SeatingMap(id, venueId, name, version, isActive);
    }

    public static EventVenue CreateEventVenue(Guid id, Guid eventId, Guid venueId, EventVenueStatus status = EventVenueStatus.Active)
    {
        return new EventVenue(id, eventId, venueId, status);
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