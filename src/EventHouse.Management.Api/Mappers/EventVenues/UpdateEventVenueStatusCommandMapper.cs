using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;

namespace EventHouse.Management.Api.Mappers.EventVenues;

internal static class UpdateEventVenueStatusCommandMapper
{
    public static UpdateEventVenueStatusCommand FromContract(Guid eventVenueId, UpdateEventVenueStatusRequest request)
    {
        return new UpdateEventVenueStatusCommand(
            eventVenueId,
            EventVenueStatusMapper.ToApplicationRequired(request.Status));
    }
}
