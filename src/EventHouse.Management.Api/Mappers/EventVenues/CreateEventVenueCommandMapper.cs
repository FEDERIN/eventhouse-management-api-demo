using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Commands.EventVenues.Create;

namespace EventHouse.Management.Api.Mappers.EventVenues;

internal static class CreateEventVenueCommandMapper
{
    public static CreateEventVenueCommand FromContract(CreateEventVenueRequest request)
        => new(
            request.EventId,
            request.VenueId,
            EventVenueStatusMapper.ToApplicationRequired(request.Status));
}
