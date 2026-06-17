using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Domain.Tests.Entities;

public sealed class GenreTests
{
    [Fact]
    public void Should_throw_when_id_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Genre(Guid.Empty, "Rock"));

        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Should_throw_when_name_is_empty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new Genre(Guid.NewGuid(), ""));
        
        Assert.Equal("name", ex.ParamName);
    }
}
