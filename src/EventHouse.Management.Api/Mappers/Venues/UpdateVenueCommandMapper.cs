using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Application.Commands.Venues.Update;

namespace EventHouse.Management.Api.Mappers.Venues;

internal static class UpdateVenueCommandMapper
{
    public static UpdateVenueCommand FromContract(Guid  venueId,UpdateVenueRequest request)
        => new(
            venueId,
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
