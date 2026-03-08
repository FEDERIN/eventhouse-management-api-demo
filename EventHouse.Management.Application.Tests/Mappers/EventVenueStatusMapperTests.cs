using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers.EventVenues;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class EventVenueStatusMapperTests
{
    [Fact]
    public void ToDomainRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((EventVenueStatusDto)999);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventVenueStatusMapper.ToDomainRequired(invalid));
    }

    [Fact]
    public void ToDomainOptional_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((EventVenueStatusDto)999);
        EventVenueStatusDto? nullable = invalid;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventVenueStatusMapper.ToDomainOptional(nullable));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidDomain_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((EventVenueStatus)999);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventVenueStatusMapper.ToApplicationRequired(invalid));
    }
}
