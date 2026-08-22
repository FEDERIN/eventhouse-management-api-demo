using EventHouse.Management.Application.Queries.Events.GetById;

namespace EventHouse.Management.Api.Mappers.Events;

internal static class GetEventByIdQueryMapper
{
    public static GetEventByIdQuery FromContract(Guid eventId)
        => new(eventId);
}
