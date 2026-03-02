using EventHouse.Management.Api.Mappers.Enums;
using Contract = EventHouse.Management.Api.Contracts.Events.EventScope;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class EventScopeMapperTests
{
    [Theory]
    [InlineData(Contract.Local, EventScopeDto.Local)]
    [InlineData(Contract.National, EventScopeDto.National)]
    [InlineData(Contract.International, EventScopeDto.International)]
    public void ToApplicationRequired_WhenValidContract_ReturnsMappedDto(
        Contract input,
        EventScopeDto expected)
    {
        var result = EventScopeMapper.ToApplicationRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplicationOptional_WhenNull_ReturnsNull()
    {
        var result = EventScopeMapper.ToApplicationOptional(null);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(Contract.Local, EventScopeDto.Local)]
    [InlineData(Contract.National, EventScopeDto.National)]
    [InlineData(Contract.International, EventScopeDto.International)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        Contract input,
        EventScopeDto expected)
    {
        Contract? nullable = input;

        var result = EventScopeMapper.ToApplicationOptional(nullable);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EventScopeDto.Local, Contract.Local)]
    [InlineData(EventScopeDto.National, Contract.National)]
    [InlineData(EventScopeDto.International, Contract.International)]
    public void ToContractRequired_WhenValidDto_ReturnsMappedContract(
        EventScopeDto input,
        Contract expected)
    {
        var result = EventScopeMapper.ToContractRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((EventScopeDto)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventScopeMapper.ToContractRequired(invalid));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidContract_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((Contract)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventScopeMapper.ToApplicationRequired(invalid));
    }
}
