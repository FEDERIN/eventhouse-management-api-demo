using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenues.Create;

internal sealed class CreateEventVenueCommandValidator
    : EventVenueCommandValidatorBase<CreateEventVenueCommand>
{
    public CreateEventVenueCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("The EventId cannot be empty.");
        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("The VenueId cannot be empty.");
        ApplyEventVenueRules(x => x.Status);
    }
}

