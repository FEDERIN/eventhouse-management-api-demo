using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Contracts.Seating.Structure;
using EventHouse.Management.Application.DTOs.Seating;
using EventHouse.Management.Application.DTOs.Seating.Structure;

namespace EventHouse.Management.Api.Mappers.Seating.Maps;

internal static class SeatingMapStructureMapper
{
    public static SeatingMapStructureResponse ToResponse(
        SeatingMapStructureDto dto)
        => new()
        {
            Id = dto.Id,
            VenueId = dto.VenueId,
            Name = dto.Name,
            Version = dto.Version,
            IsActive = dto.IsActive,
            CreatedAtUtc = dto.CreatedAtUtc,
            Sections = [.. dto.Sections.Select(ToSectionResponse)]
        };

    private static SeatingSectionStructureResponse ToSectionResponse(
        SeatingSectionStructureDto dto)
        => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            IsNumbered = dto.IsNumbered,
            Capacity = dto.Capacity,
            IsActive = dto.IsActive,
            Rows = [.. dto.Rows.Select(ToRowResponse)]
        };

    private static SeatingRowStructureResponse ToRowResponse(
        SeatingRowStructureDto dto)
        => new()
        {
            Id = dto.Id,
            Number = dto.Number,
            Label = dto.Label,
            IsActive = dto.IsActive,
            Seats = [.. dto.Seats.Select(ToSeatResponse)]
        };

    private static SeatResponse ToSeatResponse(
        SeatDto dto)
        => new()
        {
            Id = dto.Id,
            Number = dto.Number,
            Label = dto.Label,
            IsActive = dto.IsActive
        };
}