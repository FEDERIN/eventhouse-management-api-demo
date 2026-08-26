using EventHouse.Management.Application.Commands.Seating.Maps.UpdateStatus;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Maps.UpdateStatus;

public sealed class UpdateSeatingMapStatusCommandValidatorTests
{
    private readonly UpdateSeatingMapStatusCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_SeatingMapId_Is_Empty()
    {
        var command = new UpdateSeatingMapStatusCommand(
            Guid.Empty,
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingMapId)
            .WithErrorMessage("SeatingMapId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateSeatingMapStatusCommand(
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.SeatingMapId);
    }
}