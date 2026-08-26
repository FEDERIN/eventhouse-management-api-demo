using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Maps.UpdateStatus;

internal sealed class UpdateSeatingMapStatusCommandValidator
    : AbstractValidator<UpdateSeatingMapStatusCommand>
{
    public UpdateSeatingMapStatusCommandValidator()
    {
        RuleFor(x => x.SeatingMapId)
            .NotEmpty()
            .WithMessage("SeatingMapId must be a non-empty GUID.");
    }
}