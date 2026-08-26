using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Rows.Add;

internal sealed class AddSeatingRowCommandValidator
    : SeatingRowCommandValidatorBase<AddSeatingRowCommand>
{
    public AddSeatingRowCommandValidator()
    {
        ApplySeatingRowRules(
            x => x.SeatingMapId,
            x => x.SeatingSectionId);

        RuleFor(x => x.Number)
            .GreaterThan(0)
            .WithMessage("Row number must be greater than zero.");

        RuleFor(x => x.Label)
            .NotEmpty()
            .WithMessage("Row label is required.")
            .MaximumLength(200)
            .WithMessage("Row label cannot exceed 200 characters.");
    }
}