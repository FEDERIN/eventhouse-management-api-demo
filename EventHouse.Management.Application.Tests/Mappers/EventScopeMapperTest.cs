using EventHouse.Management.Application.Mappers;
using EventScopeDto = EventHouse.Management.Application.Common.Enums.EventScopeDto;
using DomainScope = EventHouse.Management.Domain.Enums.EventScope;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class EventScopeMapperTests
{
    [Theory]
    [InlineData(EventScopeDto.Local, DomainScope.Local)]
    [InlineData(EventScopeDto.National, DomainScope.National)]
    [InlineData(EventScopeDto.International, DomainScope.International)]
    public void ToDomainRequired_WhenValidEventScopeDto_ReturnsMappedDomain(
        EventScopeDto input,
        DomainScope expected)
    {
        var result = EventScopeMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EventScopeDto.Local, DomainScope.Local)]
    [InlineData(EventScopeDto.National, DomainScope.National)]
    [InlineData(EventScopeDto.International, DomainScope.International)]
    public void ToDomainOptional_WhenHasValue_ReturnsMappedDomain(
        EventScopeDto input,
        DomainScope expected)
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
    [InlineData(DomainScope.Local, EventScopeDto.Local)]
    [InlineData(DomainScope.National, EventScopeDto.National)]
    [InlineData(DomainScope.International, EventScopeDto.International)]
    public void ToApplicationRequired_WhenValidDomainScope_ReturnsMappedApp(
        DomainScope input,
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
        var invalid = unchecked((DomainScope)999);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            EventScopeMapper.ToApplicationRequired(invalid));
    }
}
