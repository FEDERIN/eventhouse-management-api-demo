using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.ArtistPerformances;

internal sealed class ArtistPerformanceMapper
{
    public static ArtistPerformanceDto ToDto(ArtistPerformance entity)
    {
        return new ArtistPerformanceDto
        {
            Id = entity.Id,
            EventVenueCalendarId = entity.EventVenueCalendarId,
            ArtistId = entity.ArtistId,
            IsHeadliner = entity.IsHeadliner,
            SetStart = entity.SetStart,
            SetEnd = entity.SetEnd
        };
    }

    public static IEnumerable<ArtistPerformanceDto> ToDto(IEnumerable<ArtistPerformance> entities)
            => entities.Select(ToDto);
}