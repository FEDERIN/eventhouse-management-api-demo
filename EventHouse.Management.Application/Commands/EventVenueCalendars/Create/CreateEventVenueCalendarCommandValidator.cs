
using FluentValidation;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars.Create;

internal sealed class CreateEventVenueCalendarCommandValidator
    : EventVenueCalendarCommandValidatorBase<CreateEventVenueCalendarCommand>
{
    public CreateEventVenueCalendarCommandValidator()
    {
        RuleFor(x => x.EventVenueId)
            .NotEmpty()
            .WithMessage("EventVenue identifier is required.");

        RuleFor(x => x.SeatingMapId)
            .NotEmpty().WithMessage("SeatingMap identifier is required.");

        RuleFor(x => x.TimeZoneId)
            .NotEmpty().WithMessage("TimeZone identifier is required.")
            .Must(BeValidIana).WithMessage("TimeZone must be a valid IANA identifier (e.g., 'Europe/Madrid').");

        ApplyEventVenueCalendarRules(x => x.StartDate,
            x => x.EndDate,
            x => x.Status
            );
    }

    private static bool BeValidIana(string? tz) =>
        !string.IsNullOrWhiteSpace(tz) && TZConvert.KnownIanaTimeZoneNames.Contains(tz);
}