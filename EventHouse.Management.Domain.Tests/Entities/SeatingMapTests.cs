using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class SeatingMapTests
{
    [Fact]
    public void Should_throw_when_id_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new SeatingMap(Guid.Empty, Guid.NewGuid(),"Test", 1,  true));

        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_venueId_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new SeatingMap(Guid.NewGuid(), Guid.Empty, "Test", 1, true));
        Assert.Equal("venueId", ex.ParamName);
    }
}
