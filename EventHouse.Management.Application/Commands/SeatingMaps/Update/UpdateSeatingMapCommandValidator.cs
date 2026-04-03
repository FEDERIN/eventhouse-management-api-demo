
using FluentValidation;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Update;

internal sealed class UpdateSeatingMapCommandValidator
    : SeatingMapCommandValidatorBase<UpdateSeatingMapCommand>
{
    public UpdateSeatingMapCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is require.");

        ApplySeatingMapRules(
            x => x.Name,
            x => x.Version
            );
    }
}
