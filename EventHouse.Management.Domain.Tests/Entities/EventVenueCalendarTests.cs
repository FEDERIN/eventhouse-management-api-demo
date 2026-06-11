using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Domain.Exceptions.Calendars;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class EventVenueCalendarTests
{
    [Fact]
    public void Should_throw_when_id_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenueCalendar(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now, "UTC", EventVenueCalendarStatus.Draft));
        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_eventVenueId_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenueCalendar(Guid.NewGuid(), Guid.Empty , Guid.NewGuid(), DateTime.Now, DateTime.Now, "UTC", EventVenueCalendarStatus.Draft));
        Assert.Equal("eventVenueId", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_seatingMapId_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, DateTime.Now, DateTime.Now, "UTC", EventVenueCalendarStatus.Draft));
        Assert.Equal("seatingMapId", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_startDate_after_endDate()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddYears(-1), "UTC", EventVenueCalendarStatus.Draft));
        Assert.Equal("The end date must be greater than the start date.", ex.Message);
    }

    [Fact]
    public void UpdateStatus_Should_Throw_PerformanceDatesRequiredException_When_Publishing_Incomplete_Performance()
    {
        var baseDate = DateTimeOffset.Parse("2024-05-20T10:00:00-05:00");
        var calendar = new EventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            baseDate, baseDate.AddDays(1), "America/Bogota", EventVenueCalendarStatus.Draft);

        var artistId = Guid.NewGuid();

        calendar.AddPerformance(artistId, true, null, null);

        var ex = Assert.Throws<PerformanceDatesRequiredException>(() =>
            calendar.UpdateStatus(EventVenueCalendarStatus.Published));

        Assert.Equal(artistId, ex.ArtistId);
    }

    [Fact]
    public void AddPerformance_Should_Throw_PerformanceDatesRequiredException_When_Calendar_Is_Published_And_Dates_Are_Null()
    {
        var baseDate = DateTimeOffset.Parse("2024-05-20T10:00:00-05:00");
        var calendar = new EventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            baseDate, baseDate.AddDays(1), "America/Bogota", EventVenueCalendarStatus.Draft);

        var validArtistId = Guid.NewGuid();
        calendar.AddPerformance(validArtistId, true, baseDate.AddHours(1), baseDate.AddHours(3));

        calendar.UpdateStatus(EventVenueCalendarStatus.Published);

        // 2 & 3. Act & Assert (Actuar y Afirmar)
        var newArtistId = Guid.NewGuid();

        var ex = Assert.Throws<PerformanceDatesRequiredException>(() =>
            calendar.AddPerformance(newArtistId, false, null, null));

        Assert.Equal(newArtistId, ex.ArtistId);
    }

    [Fact]
    public void UpdatePerformance_Should_Throw_PerformanceDatesRequiredException_When_Calendar_Is_Published_And_Dates_Are_Removed()
    {
        var baseDate = DateTimeOffset.Parse("2024-05-20T10:00:00-05:00");
        var calendar = new EventVenueCalendar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            baseDate, baseDate.AddDays(1), "America/Bogota", EventVenueCalendarStatus.Draft);

        var artistId = Guid.NewGuid();

        calendar.AddPerformance(artistId, true, baseDate.AddHours(1), baseDate.AddHours(3));

        calendar.UpdateStatus(EventVenueCalendarStatus.Published);

        var ex = Assert.Throws<PerformanceDatesRequiredException>(() =>
            calendar.UpdatePerformance(artistId, null, null));

        Assert.Equal(artistId, ex.ArtistId);
    }
}
