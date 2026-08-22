using EventHouse.Management.Application.Queries.EventVenues.GetById;

namespace EventHouse.Management.Api.Mappers.EventVenues;

internal static class GetEventVenueByIdQueryMapper
{
    public static GetEventVenueByIdQuery FromContract(Guid eventVenueId)
        => new(eventVenueId);
}
