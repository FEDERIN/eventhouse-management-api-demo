using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Update;

internal sealed class UpdateSeatingSectionCommandValidator
    : SeatingSectionCommandValidatorBase<UpdateSeatingSectionCommand>
{
    public UpdateSeatingSectionCommandValidator()
    {
        ApplySeatingSectionRules(
            x => x.SeatingMapId);

        RuleFor(x => x.SectionId)
            .NotEmpty()
            .WithMessage("SectionId must be a non-empty GUID.");

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