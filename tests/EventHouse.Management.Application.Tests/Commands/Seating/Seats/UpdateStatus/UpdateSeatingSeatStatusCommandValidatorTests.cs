using EventHouse.Management.Application.Commands.Seating.Seats.UpdateStatus;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Seats.UpdateStatus;

public sealed class UpdateSeatingSeatStatusCommandValidatorTests
{
    private readonly UpdateSeatingSeatStatusCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_SeatingMapId_Is_Empty()
    {
        var command = new UpdateSeatingSeatStatusCommand(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingMapId);
    }

    [Fact]
    public void Should_Have_Error_When_SeatingSectionId_Is_Empty()
    {
        var command = new UpdateSeatingSeatStatusCommand(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingSectionId);
    }

    [Fact]
    public void Should_Have_Error_When_SeatingRowId_Is_Empty()
    {
        var command = new UpdateSeatingSeatStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingRowId);
    }

    [Fact]
    public void Should_Have_Error_When_SeatId_Is_Empty()
    {
        var command = new UpdateSeatingSeatStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatId)
            .WithErrorMessage("SeatId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateSeatingSeatStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}