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

    /// Creates a valid ArtistGenre entity for testing purposes.
    /// </summary>
    public static ArtistGenre CreateArtistGenre(
        Guid? artistId = null,
        Guid? genreId = null,
        ArtistGenreStatus status = ArtistGenreStatus.Active,
        bool isPrimary = false)
    {
        return new ArtistGenre(
            artistId ?? Guid.NewGuid(),
            genreId ?? Guid.NewGuid(),
            status,
            isPrimary);
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
}