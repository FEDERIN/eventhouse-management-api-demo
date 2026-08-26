using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Application.Commands.Seating.Rows.UpdateStatus;

namespace EventHouse.Management.Api.Mappers.Seating.Rows;

internal static class UpdateSeatingRowStatusCommandMapper
{
    public static UpdateSeatingRowStatusCommand FromContract(
        Guid seatingMapId,
        Guid sectionId,
        Guid rowId,
        UpdateSeatingRowStatusRequest request)
    {
        return new UpdateSeatingRowStatusCommand(
            seatingMapId,
            sectionId,
            rowId,
            request.IsActive);
    }
}