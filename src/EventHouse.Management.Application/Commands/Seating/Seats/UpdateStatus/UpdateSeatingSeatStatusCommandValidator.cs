using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Seats.UpdateStatus;

internal sealed class UpdateSeatingSeatStatusCommandValidator
    : SeatingSeatCommandValidatorBase<UpdateSeatingSeatStatusCommand>
{
    public UpdateSeatingSeatStatusCommandValidator()
    {
        ApplySeatingSeatRules(
            x => x.SeatingMapId,
            x => x.SeatingSectionId,
            x => x.SeatingRowId);

        RuleFor(x => x.SeatId)
            .NotEmpty()
            .WithMessage("SeatId must be a non-empty GUID.");
    }
}