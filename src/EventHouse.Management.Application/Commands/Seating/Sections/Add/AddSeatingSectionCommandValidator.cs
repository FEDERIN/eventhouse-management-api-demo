using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Add;

internal sealed class AddSeatingSectionCommandValidator
    : SeatingSectionCommandValidatorBase<AddSeatingSectionCommand>
{
    public AddSeatingSectionCommandValidator()
    {
        ApplySeatingSectionRules(
            x => x.SeatingMapId);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than zero.");
    }
}