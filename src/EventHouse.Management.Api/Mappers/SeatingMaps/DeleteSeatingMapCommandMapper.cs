using EventHouse.Management.Application.Commands.SeatingMaps.Delete;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal static class DeleteSeatingMapCommandMapper
{
    public static DeleteSeatingMapCommand FromContract(Guid seatingMapId)
        => new(seatingMapId);
}
