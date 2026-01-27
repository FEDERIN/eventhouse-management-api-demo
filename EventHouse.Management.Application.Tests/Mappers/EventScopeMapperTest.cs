using EventHouse.Management.Application.Mappers;
using EventScopeDto = EventHouse.Management.Application.Common.Enums.EventScopeDto;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class EventScopeMapperTests
{
    [Theory]
    [InlineData(EventScopeDto.Local, EventScope.Local)]
    [InlineData(EventScopeDto.National, EventScope.National)]
    [InlineData(EventScopeDto.International, EventScope.International)]
    public void ToDomainRequired_WhenValidEventScopeDto_ReturnsMappedDomain(
        EventScopeDto input,
        EventScope expected)
    {
        var result = EventScopeMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EventScopeDto.Local, EventScope.Local)]
    [InlineData(EventScopeDto.National, EventScope.National)]
    [InlineData(EventScopeDto.International, EventScope.International)]
    public void ToDomainOptional_WhenHasValue_ReturnsMappedDomain(
        EventScopeDto input,
        EventScope expected)
    {
        var result = EventScopeMapper.ToDomainOptional(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainOptional_WhenNull_ReturnsNull()
    {
        EventScopeDto? input = null;

        var result = EventScopeMapper.ToDomainOptional(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(EventScope.Local, EventScopeDto.Local)]
    [InlineData(EventScope.National, EventScopeDto.National)]
    [InlineData(EventScope.International, EventScopeDto.International)]
    public void ToApplicationRequired_WhenValidDomainScope_ReturnsMappedApp(
        EventScope input,
        EventScopeDto expected)
    {
        var result = EventScopeMapper.ToApplicationRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainRequired_WhenInvalidValue_ShouldThrowArgumentOutOfRange()
    {
        var invalid = unchecked((EventScopeDto)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventScopeMapper.ToDomainRequired(invalid));
    }

    [Fact]
    public void ToApplicationRequired_WhenInvalidValue_ShouldThrowArgumentOutOfRange()
    {
        var invalid = unchecked((EventScope)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventScopeMapper.ToApplicationRequired(invalid));
    }
}
