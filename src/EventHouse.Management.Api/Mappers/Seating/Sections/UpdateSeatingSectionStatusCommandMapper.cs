using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Application.Commands.Seating.Sections.UpdateStatus;

namespace EventHouse.Management.Api.Mappers.Seating.Sections;

internal static class UpdateSeatingSectionStatusCommandMapper
{
    public static UpdateSeatingSectionStatusCommand FromContract(
        Guid seatingMapId,
        Guid sectionId,
        UpdateSeatingSectionStatusRequest request)
    {
        return new UpdateSeatingSectionStatusCommand(
            seatingMapId,
            sectionId,
            request.IsActive);
    }
}