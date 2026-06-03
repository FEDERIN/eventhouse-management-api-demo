using FluentValidation;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

internal sealed class CreateSeatingMapCommandValidator : SeatingMapCommandValidatorBase<CreateSeatingMapCommand>
{
    public CreateSeatingMapCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("VenueId is required.");

        ApplySeatingMapRules(x => x.Name, x => x.Version);
    }
}