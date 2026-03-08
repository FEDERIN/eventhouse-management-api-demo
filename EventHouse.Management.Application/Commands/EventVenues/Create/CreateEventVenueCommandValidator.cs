using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenues.Create;

internal sealed class CreateEventVenueCommandValidator
    : EventVenueCommandValidatorBase<CreateEventVenueCommand>
{
    public CreateEventVenueCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId no puede ser Guid.Empty.");

        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("VenueId no puede ser Guid.Empty.");
    }
}

