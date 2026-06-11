using EventHouse.Management.Application.Mappers;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Domain.Enums;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Mappers;

public class EnumMapperTests
{
    [Fact]
    public void ToDomainRequired_WhenInvalidDto_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidSortDirection = (SortDirection)999;

        // Act & Assert
        var act = () => EnumMapper<EventScope, SortDirection>
            .ToDomainRequired(invalidSortDirection);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"*Invalid SortDirection value.*");
    }

    //[Fact]
    //public void ToDomainRequired_WhenValidDto_ReturnsMappedDomain()
    //{
    //    // Arrange
    //    var validSortDirection = SortDirection.Asc;

    //    // Act
    //    var result = EnumMapper<EventScope, SortDirection>
    //        .ToDomainRequired(validSortDirection);

    //    // Assert
    //    result.Should().Be(EventScope.Local); // Asumiendo que Asc mapea a Local
    //}

    //[Fact]
    //public void ToDomainOptional_WhenNull_ReturnsNull()
    //{
    //    // Arrange
    //    SortDirection? nullValue = null;

    //    // Act
    //    var result = EnumMapper<EventScope, SortDirection>
    //        .ToDomainOptional(nullValue);

    //    // Assert
    //    result.Should().BeNull();
    //}

    //[Fact]
    //public void ToDomainOptional_WhenHasValue_ReturnsMappedDomain()
    //{
    //    // Arrange
    //    SortDirection? optionalValue = SortDirection.Asc;

    //    // Act
    //    var result = EnumMapper<EventScope, SortDirection>
    //        .ToDomainOptional(optionalValue);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result?.ToString().Should().Be(SortDirection.Asc.ToString());
    //}

    [Fact]
    public void ToApplicationRequired_WhenInvalidDomain_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidEventScope = unchecked((EventScope)999);

        // Act & Assert
        var act = () => EnumMapper<EventScope, SortDirection>
            .ToApplicationRequired(invalidEventScope);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"*Invalid EventScope value.*");
    }

    //[Fact]
    //public void ToApplicationRequired_WhenValidDomain_ReturnsMappedApp()
    //{
    //    // Arrange
    //    var validEventScope = EventScope.Local;

    //    // Act
    //    var result = EnumMapper<EventScope, SortDirection>
    //        .ToApplicationRequired(validEventScope);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.ToString().Should().Be(EventScope.Local.ToString());
    //}
}