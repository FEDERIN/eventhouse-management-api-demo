using EventHouse.Management.Application.Common;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Update
{
    public record UpdateVenueCommand(
        Guid Id,
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
        ) : IRequest<UpdateResult>;
}
