using EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;
using EventHouse.Management.Application.Common.Enums;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.EventVenues.UpdateStatus;

public sealed class UpdateEventVenueStatusValidatorTests
{
    private readonly UpdateEventVenueStatusCommandValidator _validator = new();

    [Fact]
    public void Should_PassValidation_When_CommandIsValid()
    {
        // Arrange
        var command = new UpdateEventVenueStatusCommand(
            Guid.NewGuid(),
            EventVenueStatusDto.Active);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_Id_IsEmpty()
    {
        // Arrange
        var command = new UpdateEventVenueStatusCommand(
            Guid.Empty,
            EventVenueStatusDto.Active);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("Id must be a non-empty GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Status_IsInvalid()
    {
        // Arrange
        var command = new UpdateEventVenueStatusCommand(
            Guid.NewGuid(),
            (EventVenueStatusDto)99);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("The provided status is not valid.");
    }
}
