using EventHouse.Management.Application.Common.RegularExpressions;
using FluentValidation;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.Venues;

internal abstract class VenueCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected VenueCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyVenueRules(
        Func<TCommand, string> name,
        Func<TCommand, string> address,
        Func<TCommand, string> city,
        Func<TCommand, string> region,
        Func<TCommand, string> countryCode,
        Func<TCommand, decimal?> latitude,
        Func<TCommand, decimal?> longitude,
        Func<TCommand, string?> timeZoneId,
        Func<TCommand, int?> capacity)
    {
        RuleFor(x => name(x))
            .NotEmpty()
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name is required and cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => address(x))
            .NotEmpty()
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Address is required and cannot contain only whitespace.")
            .MaximumLength(300);

        RuleFor(x => city(x))
            .NotEmpty()
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("City is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => region(x))
            .NotEmpty()
            .Must(r => !string.IsNullOrWhiteSpace(r))
            .WithMessage("Region is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => countryCode(x))
            .NotEmpty()
            .Must(cc => VenueRegex.CountryCode().IsMatch(cc.Trim().ToUpperInvariant()))
            .WithMessage("CountryCode must be a valid ISO-3166-1 alpha-2 code (e.g. 'ES').");

        RuleFor(x => latitude(x))
            .InclusiveBetween(-90m, 90m)
            .When(x => latitude(x).HasValue);

        RuleFor(x => longitude(x))
            .InclusiveBetween(-180m, 180m)
            .When(x => longitude(x).HasValue);

        RuleFor(x => timeZoneId(x))
            .Must(tz => tz is null || TZConvert.KnownIanaTimeZoneNames.Contains(tz.Trim()))
            .WithMessage("TimeZoneId must be a valid IANA time zone (e.g. 'Europe/Malta').");

        RuleFor(x => capacity(x))
            .GreaterThanOrEqualTo(0)
            .When(x => capacity(x).HasValue);
    }
}
