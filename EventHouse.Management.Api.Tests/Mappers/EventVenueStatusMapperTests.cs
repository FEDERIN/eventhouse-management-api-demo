using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class EventVenueStatusMapperTests
{   
    [Theory]
    [InlineData(EventVenueStatus.Active, EventVenueStatusDto.Active)]
    [InlineData(EventVenueStatus.Inactive, EventVenueStatusDto.Inactive)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        EventVenueStatus input,
        EventVenueStatusDto expected)
    {
        EventVenueStatus? nullable = input;
    
        var result = EventVenueStatusMapper.ToApplicationOptional(nullable);
    
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalidValue = (EventVenueStatusDto)999;

        Assert.Throws<ArgumentOutOfRangeException>(() => EventVenueStatusMapper.ToContractRequired(invalidValue));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalidValue = (EventVenueStatus)999;
        Assert.Throws<ArgumentOutOfRangeException>(() => EventVenueStatusMapper.ToApplicationRequired(invalidValue));
    }
}
