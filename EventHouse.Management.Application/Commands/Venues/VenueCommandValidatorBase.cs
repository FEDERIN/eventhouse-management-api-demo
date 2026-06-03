using EventHouse.Management.Application.Common.RegularExpressions;
using FluentValidation;
using System.Linq.Expressions;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.Venues;

internal abstract class VenueCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected VenueCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyVenueRules(
        Expression<Func<TCommand, string>> name,
        Expression<Func<TCommand, string>> address,
        Expression<Func<TCommand, string>> city,
        Expression<Func<TCommand, string>> region,
        Expression<Func<TCommand, string>> countryCode,
        Expression<Func<TCommand, decimal?>> latitude,
        Expression<Func<TCommand, decimal?>> longitude,
        Expression<Func<TCommand, string?>> timeZoneId,
        Expression<Func<TCommand, int?>> capacity)
    {
        RuleFor(name)
            .NotEmpty()
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name is required and cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(address)
            .NotEmpty()
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Address is required and cannot contain only whitespace.")
            .MaximumLength(300);

        RuleFor(city)
            .NotEmpty()
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("City is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(region)
            .NotEmpty()
            .Must(r => !string.IsNullOrWhiteSpace(r))
            .WithMessage("Region is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(countryCode)
            .NotEmpty()
            .Must(cc => VenueRegex.CountryCode().IsMatch(cc.Trim().ToUpperInvariant()))
            .WithMessage("CountryCode must be a valid ISO-3166-1 alpha-2 code (e.g. 'ES').");

        RuleFor(latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => latitude.Compile()(x).HasValue);

        RuleFor(longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => longitude.Compile()(x).HasValue);

        RuleFor(timeZoneId)
            .Must(tz => tz is null || TZConvert.KnownIanaTimeZoneNames.Contains(tz.Trim()))
            .WithMessage("TimeZoneId must be a valid IANA time zone (e.g. 'Europe/Malta').");

        RuleFor(capacity)
            .GreaterThanOrEqualTo(0)
            .When(x => capacity.Compile()(x).HasValue);
    }
}
