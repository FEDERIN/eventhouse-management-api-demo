using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.Venues;

internal class VenuesMapper
{
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
            Latitude = venue.Latitude,
            Longitude = venue.Longitude,
            TimeZoneId = venue.TimeZoneId,
            IsActive = venue.IsActive
        };
    }

    public static IEnumerable<VenueDto> ToDto(IEnumerable<Venue> venues)
    {
        return venues.Select(ToDto);
    }
}
