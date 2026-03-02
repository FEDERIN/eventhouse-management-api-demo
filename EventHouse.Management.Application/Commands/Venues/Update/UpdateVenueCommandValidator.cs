using FluentValidation;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandValidator
    : VenueCommandValidatorBase<UpdateVenueCommand>
{
    public UpdateVenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id must be a non-empty GUID.");

        ApplyVenueRules(
            x => x.Name,
            x => x.Address,
            x => x.City,
            x => x.Region,
            x => x.CountryCode,
            x => x.Latitude,
            x => x.Longitude,
            x => x.TimeZoneId,
            x => x.Capacity
        );
    }
}