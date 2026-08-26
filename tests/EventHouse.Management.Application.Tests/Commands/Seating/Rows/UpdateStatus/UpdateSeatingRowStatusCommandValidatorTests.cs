using EventHouse.Management.Application.Commands.Seating.Rows.UpdateStatus;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Rows.UpdateStatus;

public sealed class UpdateSeatingRowStatusCommandValidatorTests
{
    private readonly UpdateSeatingRowStatusCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_SeatingMapId_Is_Empty()
    {
        var command = new UpdateSeatingRowStatusCommand(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingMapId);
    }

    [Fact]
    public void Should_Have_Error_When_SeatingSectionId_Is_Empty()
    {
        var command = new UpdateSeatingRowStatusCommand(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingSectionId);
    }

    [Fact]
    public void Should_Have_Error_When_RowId_Is_Empty()
    {
        var command = new UpdateSeatingRowStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RowId)
            .WithErrorMessage("RowId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateSeatingRowStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}