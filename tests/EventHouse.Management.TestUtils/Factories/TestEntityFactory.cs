using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Tests.Shared.Factories;

public static class TestEntityFactory
{
    public static Artist CreateArtist(string name, ArtistCategory category)
    {
        var id = Guid.NewGuid();
        var uniqueName = $"{name} {Guid.NewGuid().ToString()[..4]}";

        return new Artist(id, uniqueName, category);
    }

    /// <summary>
    /// Creates a Genre entity with a randomized name to avoid collisions.
    /// </summary>
    public static Genre CreateGenre(Guid? id = null, string name = "Rock")
    {
        var genreId = id ?? Guid.NewGuid();
        var uniqueName = $"{name} {Guid.NewGuid().ToString()[..4]}";

        return new Genre(genreId, uniqueName);
    }


    public static EventVenueCalendar CreateEventVenueCalendar(
    Guid id,
    Guid eventVenueId,
    Guid seatingMapId,
    DateTimeOffset? startLocal = null,
    DateTimeOffset? endLocal = null,
    string timeZoneId = "UTC")
    {
        return new EventVenueCalendar(
            id,
            eventVenueId,
            seatingMapId,
            startLocal ?? DateTimeOffset.UtcNow,
            endLocal ?? DateTimeOffset.UtcNow.AddHours(10),
            timeZoneId);
    }
}