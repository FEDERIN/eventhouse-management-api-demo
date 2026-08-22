using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Application.Commands.Events.Create;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class CreateEventCommandMapper
{
    public static CreateEventCommand FromContract(CreateEventRequest request)
        => new(
            request.Name,
            request.Description,
            EventScopeMapper.ToApplicationRequired(request.Scope));
}