using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class ArtistPerformanceTests
{
    #region Constructor Exceptions

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEventVenueCalendarIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new ArtistPerformance(
            eventVenueCalendarId: Guid.Empty,
            artistId: Guid.NewGuid(),
            isHeadliner: true,
            setStartLocal: DateTimeOffset.UtcNow,
            setEndLocal: DateTimeOffset.UtcNow.AddHours(1)
        ));

        Assert.Equal("EventVenueCalendarId is required.", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenArtistIdIsEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new ArtistPerformance(
            eventVenueCalendarId: Guid.NewGuid(),
            artistId: Guid.Empty,
            isHeadliner: true,
            setStartLocal: DateTimeOffset.UtcNow,
            setEndLocal: DateTimeOffset.UtcNow.AddHours(1)
        ));

        Assert.Equal("ArtistId is required.", ex.Message);
    }

    #endregion

    #region ValidateTimeRange Exceptions

    [Fact]
    public void ValidateTimeRange_ShouldThrowArgumentException_WhenStartIsBeforeCalendarStart()
    {
        // Arrange
        var performance = new ArtistPerformance(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            DateTimeOffset.Parse("2024-01-01T09:00:00Z"),
            DateTimeOffset.Parse("2024-01-01T11:00:00Z")
        );

        var calendarStartUtc = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            performance.ValidateTimeRange(calendarStartUtc, null)
        );

        Assert.Equal("Performance cannot start before the calendar slot begins.", ex.Message);
    }

    [Fact]
    public void ValidateTimeRange_ShouldThrowArgumentException_WhenStartIsGreaterThanOrEqualToEnd()
    {
        var performance = new ArtistPerformance(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            DateTimeOffset.Parse("2024-01-01T12:00:00Z"),
            DateTimeOffset.Parse("2024-01-01T11:00:00Z")
        );

        var calendarStartUtc = new DateTime(2024, 1, 1, 8, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            performance.ValidateTimeRange(calendarStartUtc, null)
        );

        Assert.Equal("Start time must be earlier than end time.", ex.Message);
    }

    #endregion
}