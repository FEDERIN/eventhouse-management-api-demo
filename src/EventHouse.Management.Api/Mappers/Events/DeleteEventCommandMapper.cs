using EventHouse.Management.Application.Commands.Events.Delete;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class DeleteEventCommandMapper
{
    public static DeleteEventCommand FromContract(Guid eventId)
        => new(eventId);
}
