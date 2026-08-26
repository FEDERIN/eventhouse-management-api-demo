using EventHouse.Management.Application.Queries.SeatingMaps.GetStructure;

namespace EventHouse.Management.Api.Mappers.Seating.Maps;

internal static class GetSeatingMapStructureQueryMapper
{
    public static GetSeatingMapStructureQuery FromContract(
        Guid seatingMapId)
    {
        return new GetSeatingMapStructureQuery(
            seatingMapId);
    }
}