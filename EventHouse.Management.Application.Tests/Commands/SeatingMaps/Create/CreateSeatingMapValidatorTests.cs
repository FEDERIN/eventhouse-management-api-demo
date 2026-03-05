
using EventHouse.Management.Application.Commands.SeatingMaps.Create;
using FluentAssertions;

namespace EventHouse.Management.Application.Tests.Commands.SeatingMaps.Create;

public sealed class CreateSeatingMapValidatorTests
{
    private readonly CreateSeatingMapCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = ValidCommand();
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void EmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { Name = string.Empty };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("NotEmptyValidator") && e.ErrorMessage.Length > 0);
     }

    [Fact]
    public void NullName_ShouldHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { Name = null! };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("NotEmptyValidator") && e.ErrorMessage.Length > 0);
    }

    [Fact]
    public void NameWithOnlyWhitespace_ShouldHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { Name = "   " };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("NotEmptyValidator") && e.ErrorMessage.Length > 0);
    }

    [Fact]
    public void EmptyVenueId_ShouldHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { VenueId = Guid.Empty };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateSeatingMapCommand.VenueId));
    }

    [Fact]
    public void ValidVenueId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { VenueId = Guid.NewGuid() };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void VersionLessThanOne_ShouldHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { Version = 0 };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("GreaterThanValidator") && e.ErrorMessage.Length > 0);
    }

    [Fact]
    public void VersionGreaterThanOne_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { Version = 2 };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsActive_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = ValidCommand() with { IsActive = false };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    private static CreateSeatingMapCommand ValidCommand() =>
        new(
            VenueId: Guid.NewGuid(),
            Name: "Main Hall",
            Version: 1,
            IsActive: true
        );
}
