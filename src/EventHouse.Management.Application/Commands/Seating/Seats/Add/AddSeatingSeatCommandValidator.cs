using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Seats.Add;

internal sealed class AddSeatingSeatCommandValidator
    : SeatingSeatCommandValidatorBase<AddSeatingSeatCommand>
{
    public AddSeatingSeatCommandValidator()
    {
        ApplySeatingSeatRules(
            x => x.SeatingMapId,
            x => x.SeatingSectionId,
            x => x.SeatingRowId);

        RuleFor(x => x.Number)
            .GreaterThan(0)
            .WithMessage("Seat number must be greater than zero.");

        RuleFor(x => x.Label)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Seat label is required and must not exceed 200 characters.");
    }
}