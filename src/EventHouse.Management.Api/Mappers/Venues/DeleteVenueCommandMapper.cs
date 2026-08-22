using EventHouse.Management.Application.Commands.Venues.Delete;

namespace EventHouse.Management.Api.Mappers.Venues;

internal static class DeleteVenueCommandMapper
{
    public static DeleteVenueCommand FromContract(Guid venueId)
        => new(venueId);
}
