using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Application.Commands.Seating.Sections.Add;

namespace EventHouse.Management.Api.Mappers.Seating.Sections;

internal static class AddSeatingSectionCommandMapper
{
    public static AddSeatingSectionCommand FromContract(
        Guid seatingMapId,
        AddSeatingSectionRequest request)
        => new(
            seatingMapId,
            request.Name,
            request.IsNumbered,
            request.Capacity);
}