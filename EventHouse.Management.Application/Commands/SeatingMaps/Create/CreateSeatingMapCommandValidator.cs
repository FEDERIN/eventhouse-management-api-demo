using FluentValidation;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

internal sealed class CreateSeatingMapCommandValidator 
    : AbstractValidator<CreateSeatingMapCommand>
{
    public CreateSeatingMapCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is require.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("VenueId is require.");

        RuleFor(x => x.Version)
            .GreaterThan(0).WithMessage("Version must be greater than 0.");
    }
}
