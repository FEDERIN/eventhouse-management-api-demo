using EventHouse.Management.Application.Common.RegularExpressions;
using FluentValidation;
using System.Text.RegularExpressions;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.Venues.Create;

public sealed class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name is required and cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .Must(a => a is null || !string.IsNullOrWhiteSpace(a))
            .WithMessage("Address cannot contain only whitespace.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .Must(c => c is null || !string.IsNullOrWhiteSpace(c))
            .WithMessage("City cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => x.Region)
            .Must(r => r is null || !string.IsNullOrWhiteSpace(r))
            .WithMessage("Region cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => x.CountryCode)
            .Must(cc => VenueRegex.CountryCode().IsMatch(cc.Trim().ToUpperInvariant()))
            .WithMessage("CountryCode must be a valid ISO-3166-1 alpha-2 code (e.g. 'ES').");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.TimeZoneId)
            .Must(tz => tz is null || TZConvert.KnownIanaTimeZoneNames.Contains(tz))
            .WithMessage("TimeZoneId must be a valid IANA time zone (e.g. 'Europe/Malta').");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Capacity.HasValue);
    }
}
