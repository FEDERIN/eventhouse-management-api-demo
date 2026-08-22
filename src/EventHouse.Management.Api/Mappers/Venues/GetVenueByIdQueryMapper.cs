using EventHouse.Management.Application.Queries.Venues.GetById;

namespace EventHouse.Management.Api.Mappers.Venues;

internal static class GetVenueByIdQueryMapper
{
    public static GetVenueByIdQuery FromContract(Guid venueId)
        => new(venueId);
}
