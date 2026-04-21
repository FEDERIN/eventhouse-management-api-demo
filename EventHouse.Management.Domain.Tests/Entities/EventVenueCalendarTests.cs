using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

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
}
