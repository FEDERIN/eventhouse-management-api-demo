using FluentValidation;
using TimeZoneConverter;

namespace EventHouse.Management.Application.Commands.Venues.Create;

public sealed class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name no puede estar vacío o solo espacios.")
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .Must(a => a is null || !string.IsNullOrWhiteSpace(a))
            .WithMessage("Address no puede ser solo espacios.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .Must(c => c is null || !string.IsNullOrWhiteSpace(c))
            .WithMessage("City no puede ser solo espacios.")
            .MaximumLength(120);

        RuleFor(x => x.Region)
            .Must(r => r is null || !string.IsNullOrWhiteSpace(r))
            .WithMessage("Region no puede ser solo espacios.")
            .MaximumLength(120);

        RuleFor(x => x.CountryCode)
            .Must(cc => cc is null || System.Text.RegularExpressions.Regex.IsMatch(cc.Trim().ToUpperInvariant(), "^[A-Z]{2}$"))
            .WithMessage("CountryCode debe ser ISO-3166-1 alpha-2 (ej: 'ES').");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.TimeZoneId)
            .Must(tz => tz is null || TZConvert.KnownIanaTimeZoneNames.Contains(tz.Trim()))
            .WithMessage("TimeZoneId debe ser una zona horaria IANA válida (ej: 'Europe/Malta').");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Capacity.HasValue);
    }
}
