using FluentValidation;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

internal sealed class CreateSeatingMapCommandValidator 
    : SeatingMapCommandValidatorBase<CreateSeatingMapCommand>
{
    public CreateSeatingMapCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("VenueId is require.");

        ApplySeatingMapRules(
            x => x.Name,
            x => x.Version
            );
    }
}
