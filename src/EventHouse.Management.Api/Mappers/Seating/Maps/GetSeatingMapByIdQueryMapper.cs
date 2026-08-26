using EventHouse.Management.Application.Queries.SeatingMaps.GetById;

namespace EventHouse.Management.Api.Mappers.Seating.Maps;

internal static class GetSeatingMapByIdQueryMapper
{
    public static GetSeatingMapByIdQuery FromContract(Guid seatingMapId)
        => new(seatingMapId);
}
