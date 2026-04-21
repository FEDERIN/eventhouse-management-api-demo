using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Update;

internal sealed class UpdateEventVenueCalendarCommandValidator
    : EventVenueCalendarCommandValidatorBase<UpdateEventVenueCalendarCommand>
{
    public UpdateEventVenueCalendarCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The calendar event identifier must not be empty.");

        ApplyEventVenueCalendarRules(x => x.StartDate,
            x => x.EndDate,
            x => x.Status
            );
    }
}