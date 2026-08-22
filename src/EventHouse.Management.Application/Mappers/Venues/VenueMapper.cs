using EventHouse.Management.Application.Commands.Venues.Create;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.Venues;

internal class VenueMapper
{
    public static Venue ToEntity(CreateVenueCommand request)
    {
        return new Venue(
            id: Guid.NewGuid(),
            name: request.Name,
            address: request.Address,
            city: request.City,
            region: request.Region,
            countryCode: request.CountryCode,
            latitude: request.Latitude,
            longitude: request.Longitude,
            timeZoneId: request.TimeZoneId,
            capacity: request.Capacity,
            isActive: request.IsActive
        );
    }

    public static VenueDto ToDto(Venue venue)
    {
        return new VenueDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Address = venue.Address,
            City = venue.City,
            Region = venue.Region,
            CountryCode = venue.CountryCode,
            Capacity = venue.Capacity,
            Latitude = venue.Coordinates.Latitude,
            Longitude = venue.Coordinates.Longitude,
            TimeZoneId = venue.TimeZoneId.Value,
            IsActive = venue.IsActive
        };
    }

    public static IEnumerable<VenueDto> ToDto(IEnumerable<Venue> venues) => venues.Select(ToDto);
}
