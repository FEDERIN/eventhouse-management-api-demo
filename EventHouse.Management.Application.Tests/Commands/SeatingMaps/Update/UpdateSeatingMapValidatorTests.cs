using EventHouse.Management.Application.Commands.SeatingMaps.Update;
using FluentAssertions;


namespace EventHouse.Management.Application.Tests.Commands.SeatingMaps.Update;

public sealed class UpdateSeatingMapValidatorTests
{
    private readonly UpdateSeatingMapCommandValidator _validator = new();
    
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
    public void InvalidCommand_ShouldHaveValidationErrors()
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
    public void VersionLessThanOne_ShouldHaveValidationErrors()
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
    public void NameExceedsMaxLength_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = ValidCommand() with { Name = new string('A', 201) };
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode.Contains("MaximumLengthValidator") && e.ErrorMessage.Length > 0);
    }

    private static UpdateSeatingMapCommand ValidCommand() => new(
        Id: Guid.NewGuid(),
        Name: "Valid Seating Map Name",
        Version: 1,
        IsActive: true
    );
}
