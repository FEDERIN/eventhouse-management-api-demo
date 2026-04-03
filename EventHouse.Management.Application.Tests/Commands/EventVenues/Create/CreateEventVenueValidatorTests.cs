using EventHouse.Management.Application.Commands.EventVenues.Create;
using EventHouse.Management.Application.Common.Enums;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.EventVenues.Create;

public sealed class CreateEventVenueValidatorTests
{
    private readonly CreateEventVenueCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_EventId_IsEmpty()
    {
        // Arrange
        var command = new CreateEventVenueCommand(Guid.Empty, Guid.NewGuid(), EventVenueStatusDto.Active);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EventId)
              .WithErrorMessage("The EventId cannot be empty.");
    }

    [Fact]
    public void Should_HaveError_When_VenueId_IsEmpty()
    {
        // Arrange
        var command = new CreateEventVenueCommand(Guid.NewGuid(), Guid.Empty, EventVenueStatusDto.Active);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VenueId)
              .WithErrorMessage("The VenueId cannot be empty.");
    }

    [Fact]
    public void Should_NotHaveError_When_Command_IsValid()
    {
        // Arrange
        var command = new CreateEventVenueCommand(Guid.NewGuid(), Guid.NewGuid(), EventVenueStatusDto.Active);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_Status_IsInvalid()
    {
        // Arrange
        var command = new CreateEventVenueCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            (EventVenueStatusDto)99);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("The provided status is not valid.");
    }
}