using EventHouse.Management.Application.Commands.Seating.Maps.Create;
using EventHouse.Management.Application.DTOs.Seating;
using EventHouse.Management.Application.DTOs.Seating.Structure;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.SeatingMaps;

internal sealed class SeatingMapMapper
{
    public static SeatingMap ToEntity(CreateSeatingMapCommand request)
    {
        return new SeatingMap(
             Guid.NewGuid(),
            request.VenueId,
            request.Name,
            request.Version == 0 ? 1 : request.Version
        );
    }

    public static SeatingMapDto ToDto(SeatingMap entity)
    {
        return new SeatingMapDto
        {
            Id = entity.Id,
            VenueId = entity.VenueId,
            Name = entity.Name,
            Version = entity.Version,
            IsActive = entity.IsActive,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }

    public static IEnumerable<SeatingMapDto> ToDto(IEnumerable<SeatingMap> seatingMaps) => seatingMaps.Select(ToDto);

    public static SeatingMapStructureDto ToStructureDto(
        SeatingMap entity)
    {
        return new SeatingMapStructureDto
        {
            Id = entity.Id,
            VenueId = entity.VenueId,
            Name = entity.Name,
            Version = entity.Version,
            IsActive = entity.IsActive,
            CreatedAtUtc = entity.CreatedAtUtc,

            Sections = [.. entity.Sections
                .Select(section => new SeatingSectionStructureDto
                {
                    Id = section.Id,
                    SeatingMapId = section.SeatingMapId,
                    Name = section.Name,
                    IsNumbered = section.IsNumbered,
                    Capacity = section.Capacity,
                    IsActive = section.IsActive,
                    Rows = [.. section.Rows
                        .Select(row => new SeatingRowStructureDto
                        {
                            Id = row.Id,
                            SeatingSectionId = row.SeatingSectionId,
                            Number = row.Number,
                            Label = row.Label,
                            IsActive = row.IsActive,
                            Seats = [.. row.Seats
                                .Select(seat => new SeatDto
                                {
                                    Id = seat.Id,
                                    SeatingRowId = seat.SeatingRowId,
                                    Number = seat.Number,
                                    Label = seat.Label,
                                    IsActive = seat.IsActive,
                                })]
                        })]
                })]
        };
    }
}
