using EventHouse.Management.Application.Commands.SeatingMaps.Create;
using EventHouse.Management.Application.DTOs;
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
}
