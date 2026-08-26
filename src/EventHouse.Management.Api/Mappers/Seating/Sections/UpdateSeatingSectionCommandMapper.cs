using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Application.Commands.Seating.Sections.Update;

namespace EventHouse.Management.Api.Mappers.Seating.Sections;

internal static class UpdateSeatingSectionCommandMapper
{
    public static UpdateSeatingSectionCommand FromContract(
        Guid seatingMapId,
        Guid sectionId,
        UpdateSeatingSectionRequest request)
        => new(
            seatingMapId,
            sectionId,
            request.Name,
            request.Capacity);
}