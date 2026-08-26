using EventHouse.Management.Application.Commands.Seating.Sections.UpdateStatus;
using FluentValidation.TestHelper;

namespace EventHouse.Management.Application.Tests.Commands.Seating.Sections.UpdateStatus;

public sealed class UpdateSeatingSectionStatusCommandValidatorTests
{
    private readonly UpdateSeatingSectionStatusCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_SeatingMapId_Is_Empty()
    {
        var command = new UpdateSeatingSectionStatusCommand(
            Guid.Empty,
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SeatingMapId);
    }

    [Fact]
    public void Should_Have_Error_When_SectionId_Is_Empty()
    {
        var command = new UpdateSeatingSectionStatusCommand(
            Guid.NewGuid(),
            Guid.Empty,
            true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SectionId)
            .WithErrorMessage("SectionId must be a non-empty GUID.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new UpdateSeatingSectionStatusCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}