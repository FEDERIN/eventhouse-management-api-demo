using EventHouse.Management.Api.Mappers;
using EventHouse.Management.Api.Contracts.Common;
using FluentAssertions;

namespace EventHouse.Management.Api.Tests.Mappers;

public class ApiEnumMapperTests
{
    [Fact]
    public void ToApplicationRequired_WhenInvalidEnumValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        // Crear un valor inválido que no existe en el enum
        var invalidSortDirection = (SortDirection)999;

        // Act & Assert
        var act = () => ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
            .ToApplicationRequired(invalidSortDirection);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"*Invalid SortDirection value.*");
    }

    //[Fact]
    //public void ToApplicationRequired_WhenValidEnumValue_MapsSuccessfully()
    //{
    //    // Arrange
    //    var validSortDirection = SortDirection.Asc;

    //    // Act
    //    var result = ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
    //        .ToApplicationRequired(validSortDirection);

    //    // Assert
    //    result.Should().Be(Application.Common.Sorting.SortDirection.Asc);
    //}

    //[Fact]
    //public void ToApplicationOptional_WhenNull_ReturnsNull()
    //{
    //    // Arrange
    //    SortDirection? nullValue = null;

    //    // Act
    //    var result = ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
    //        .ToApplicationOptional(nullValue);

    //    // Assert
    //    result.Should().BeNull();
    //}

    //[Fact]
    //public void ToApplicationOptional_WhenHasValue_MapSuccessfully()
    //{
    //    // Arrange
    //    SortDirection? optionalValue = SortDirection.Desc;

    //    // Act
    //    var result = ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
    //        .ToApplicationOptional(optionalValue);

    //    // Assert
    //    result.Should().Be(Application.Common.Sorting.SortDirection.Desc);
    //}

    [Fact]
    public void ToContract_WhenInvalidEnumValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidDto = (Application.Common.Sorting.SortDirection)999;

        // Act & Assert
        var act = () => ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
            .ToContract(invalidDto);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"*Invalid SortDirection value.*");
    }

    //[Fact]
    //public void ToContract_WhenValidEnumValue_MapsSuccessfully()
    //{
    //    // Arrange
    //    var validDto = Application.Common.Sorting.SortDirection.Asc;

    //    // Act
    //    var result = ApiEnumMapper<SortDirection, Application.Common.Sorting.SortDirection>
    //        .ToContract(validDto);

    //    // Assert
    //    result.Should().Be(SortDirection.Asc);
    //}
}