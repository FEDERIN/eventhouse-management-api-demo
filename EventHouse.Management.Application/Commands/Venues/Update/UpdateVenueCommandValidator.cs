using EventHouse.Management.Application.Common.RegularExpressions;
using FluentValidation;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandValidator : AbstractValidator<UpdateVenueCommand>
{
    public UpdateVenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id must be a non-empty GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name is required and cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(a => !string.IsNullOrWhiteSpace(a))
            .WithMessage("Address is required and cannot contain only whitespace.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .NotEmpty()
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("City is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => x.Region)
            .NotEmpty()
            .Must(r => !string.IsNullOrWhiteSpace(r))
            .WithMessage("Region is required and cannot contain only whitespace.")
            .MaximumLength(120);

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .Must(cc => VenueRegex.CountryCode().IsMatch(cc.Trim().ToUpperInvariant()))
            .WithMessage("CountryCode must be a valid ISO-3166-1 alpha-2 code (e.g. 'ES').");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.TimeZoneId)
            .Must(tz => tz is null || TZConvert.KnownIanaTimeZoneNames.Contains(tz.Trim()))
            .WithMessage("TimeZoneId must be a valid IANA time zone (e.g. 'Europe/Malta').");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Capacity.HasValue);
    }
}
