using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Commands.Events.Update;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class UpdateEventCommandMapper
{
    public static UpdateEventCommand FromContract(Guid eventId, UpdateEventRequest request)
        => new(
            eventId,
            request.Name,
            request.Description,
            EventScopeMapper.ToApplicationRequired(request.Scope));
}
