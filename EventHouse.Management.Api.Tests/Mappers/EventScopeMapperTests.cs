using EventHouse.Management.Api.Mappers.Enums;
using Contract = EventHouse.Management.Api.Contracts.Events.EventScope;
using Dto = EventHouse.Management.Application.Common.Enums.EventScope;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class EventScopeMapperTests
{
    [Theory]
    [InlineData(Contract.Local, Dto.Local)]
    [InlineData(Contract.National, Dto.National)]
    [InlineData(Contract.International, Dto.International)]
    public void ToApplicationRequired_WhenValidContract_ReturnsMappedDto(
        Contract input,
        Dto expected)
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
    [InlineData(Contract.Local, Dto.Local)]
    [InlineData(Contract.National, Dto.National)]
    [InlineData(Contract.International, Dto.International)]
    public void ToApplicationOptional_WhenHasValue_ReturnsMappedDto(
        Contract input,
        Dto expected)
    {
        Contract? nullable = input;

        var result = EventScopeMapper.ToApplicationOptional(nullable);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Dto.Local, Contract.Local)]
    [InlineData(Dto.National, Contract.National)]
    [InlineData(Dto.International, Contract.International)]
    public void ToContractRequired_WhenValidDto_ReturnsMappedContract(
        Dto input,
        Contract expected)
    {
        var result = EventScopeMapper.ToContractRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToContractRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        var invalid = unchecked((Dto)999);

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
