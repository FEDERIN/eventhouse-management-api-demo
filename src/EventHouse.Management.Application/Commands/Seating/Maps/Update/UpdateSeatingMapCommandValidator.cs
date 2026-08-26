using FluentValidation;

namespace EventHouse.Management.Application.Commands.Seating.Maps.Update;

internal sealed class UpdateSeatingMapCommandValidator
    : SeatingMapCommandValidatorBase<UpdateSeatingMapCommand>
{
    public UpdateSeatingMapCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        ApplySeatingMapRules(
            x => x.Name,
            x => x.Version
            );
    }
}
