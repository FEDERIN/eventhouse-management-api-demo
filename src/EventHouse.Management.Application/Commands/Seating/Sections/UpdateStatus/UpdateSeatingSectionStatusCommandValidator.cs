using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Sections.UpdateStatus;

internal sealed class UpdateSeatingSectionStatusCommandValidator
    : SeatingSectionCommandValidatorBase<UpdateSeatingSectionStatusCommand>
{
    public UpdateSeatingSectionStatusCommandValidator()
    {
        ApplySeatingSectionRules(
            x => x.SeatingMapId);

        RuleFor(x => x.SectionId)
            .NotEmpty()
            .WithMessage("SectionId must be a non-empty GUID.");
    }
}