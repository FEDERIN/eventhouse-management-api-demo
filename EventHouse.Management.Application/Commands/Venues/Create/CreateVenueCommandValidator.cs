
namespace EventHouse.Management.Application.Commands.Venues.Create;

internal sealed class CreateVenueCommandValidator
    : VenueCommandValidatorBase<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
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