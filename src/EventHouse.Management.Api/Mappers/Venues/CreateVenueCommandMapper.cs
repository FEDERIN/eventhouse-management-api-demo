using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Application.Commands.Venues.Create;

namespace EventHouse.Management.Api.Mappers.Venues;

internal static class CreateVenueCommandMapper
{
    public static CreateVenueCommand FromContract(CreateVenueRequest request)
        => new(
            request.Name,
            request.Address,
            request.City,
            request.Region,
            request.CountryCode,
            request.Latitude,
            request.Longitude,
            request.TimeZoneId,
            request.Capacity,
            request.IsActive);
}
