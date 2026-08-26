using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Rows.UpdateStatus;

internal sealed class UpdateSeatingRowStatusCommandValidator
    : SeatingRowCommandValidatorBase<UpdateSeatingRowStatusCommand>
{
    public UpdateSeatingRowStatusCommandValidator()
    {
        ApplySeatingRowRules(
            x => x.SeatingMapId,
            x => x.SeatingSectionId);

        RuleFor(x => x.RowId)
            .NotEmpty()
            .WithMessage("RowId must be a non-empty GUID.");
    }
}