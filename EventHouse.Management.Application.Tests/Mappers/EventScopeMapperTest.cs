using EventHouse.Management.Application.Mappers;
using AppScope = EventHouse.Management.Application.Common.Enums.EventScope;
using DomainScope = EventHouse.Management.Domain.Enums.EventScope;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class EventScopeMapperTests
{
    [Theory]
    [InlineData(AppScope.Local, DomainScope.Local)]
    [InlineData(AppScope.National, DomainScope.National)]
    [InlineData(AppScope.International, DomainScope.International)]
    public void ToDomainRequired_WhenValidAppScope_ReturnsMappedDomain(
        AppScope input,
        DomainScope expected)
    {
        var result = EventScopeMapper.ToDomainRequired(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(AppScope.Local, DomainScope.Local)]
    [InlineData(AppScope.National, DomainScope.National)]
    [InlineData(AppScope.International, DomainScope.International)]
    public void ToDomainOptional_WhenHasValue_ReturnsMappedDomain(
        AppScope input,
        DomainScope expected)
    {
        var result = EventScopeMapper.ToDomainOptional(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainOptional_WhenNull_ReturnsNull()
    {
        AppScope? input = null;

        var result = EventScopeMapper.ToDomainOptional(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(DomainScope.Local, AppScope.Local)]
    [InlineData(DomainScope.National, AppScope.National)]
    [InlineData(DomainScope.International, AppScope.International)]
    public void ToApplicationRequired_WhenValidDomainScope_ReturnsMappedApp(
        DomainScope input,
        AppScope expected)
    {
        var result = EventScopeMapper.ToApplicationRequired(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToDomainRequired_WhenInvalidValue_ShouldThrowArgumentOutOfRange()
    {
        var invalid = unchecked((AppScope)999);

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
