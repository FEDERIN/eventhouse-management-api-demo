using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;


namespace EventHouse.Management.Application.Mappers.SeatingMaps;

internal sealed class SeatingMapMapper
{
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

    public static IReadOnlyList<SeatingMapDto> ToDto(IReadOnlyList<SeatingMap> seatingMaps)
    {
        return [.. seatingMaps.Select(ToDto)];
    }
}
