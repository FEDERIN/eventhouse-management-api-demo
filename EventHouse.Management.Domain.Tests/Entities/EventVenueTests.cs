using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class EventVenueTests
{
    [Fact]
    public void Should_throw_when_id_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenue(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), EventVenueStatus.Active));
        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_eventId_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenue(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), EventVenueStatus.Active));
        Assert.Equal("eventId", ex.ParamName);
    }
    
    [Fact]
    public void Should_throw_when_venueId_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new EventVenue(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, EventVenueStatus.Active));
        Assert.Equal("venueId", ex.ParamName);
    }
}
