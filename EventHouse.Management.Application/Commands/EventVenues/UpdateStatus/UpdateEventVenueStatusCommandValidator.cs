using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;

internal class UpdateEventVenueStatusCommandValidator
    : EventVenueCommandValidatorBase<UpdateEventVenueStatusCommand>
{
    public UpdateEventVenueStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id no puede ser Guid.Empty.");
    }
}
