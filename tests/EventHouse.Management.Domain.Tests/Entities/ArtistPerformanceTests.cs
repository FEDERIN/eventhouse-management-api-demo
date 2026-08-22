using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Exceptions.Calendars;
using EventHouse.Management.Domain.ValueObjects;

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

        Assert.Equal("Value does not fall within the expected range. (Parameter 'eventVenueCalendarId')",
            ex.Message);
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

        Assert.Equal("Value does not fall within the expected range. (Parameter 'artistId')",
            ex.Message);
    }

    #endregion

    #region ValidateTimeRange Exceptions

    [Fact]
    public void ValidateTimeRange_ShouldThrow_WhenPerformanceStartsBeforeCalendar()
    {
        var performance = new ArtistPerformance(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            DateTimeOffset.Parse("2024-01-01T09:00:00Z"),
            DateTimeOffset.Parse("2024-01-01T11:00:00Z"));

        var calendarRange = TimeRange.Create(
            new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 1, 20, 0, 0, DateTimeKind.Utc));

        Assert.Throws<PerformanceOutsideCalendarException>(() =>
            performance.ValidateTimeRange(calendarRange));
    }

    [Fact]
    public void ValidateTimeRange_ShouldNotThrow_WhenPerformanceIsWithinCalendar()
    {
        var performance = new ArtistPerformance(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            DateTimeOffset.Parse("2024-01-01T11:00:00Z"),
            DateTimeOffset.Parse("2024-01-01T12:00:00Z"));

        var calendarRange = TimeRange.Create(
            new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 1, 20, 0, 0, DateTimeKind.Utc));

        var exception = Record.Exception(() =>
            performance.ValidateTimeRange(calendarRange));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateTimeRange_ShouldThrow_WhenPerformanceEndsAfterCalendar()
    {
        var performance = new ArtistPerformance(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            DateTimeOffset.Parse("2024-01-01T19:00:00Z"),
            DateTimeOffset.Parse("2024-01-01T21:00:00Z"));

        var calendarRange = TimeRange.Create(
            new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 1, 20, 0, 0, DateTimeKind.Utc));

        Assert.Throws<PerformanceOutsideCalendarException>(() =>
            performance.ValidateTimeRange(calendarRange));
    }

    #endregion
}