using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Create
{
    public record CreateVenueCommand(
        string Name,
        string Address,
        string City,
        string Region,
        string CountryCode,
        decimal? Latitude,
        decimal? Longitude,
        string? TimeZoneId,
        int? Capacity,
        bool IsActive
    ) : IRequest<VenueDto>;
}
