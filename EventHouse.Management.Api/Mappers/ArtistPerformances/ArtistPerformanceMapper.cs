using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.ArtistPerformances;

public class ArtistPerformanceMapper
{
   public static ArtistPerformanceResponse ToContract(ArtistPerformanceDto dto)
    {
        return new ArtistPerformanceResponse
        {
            Id = dto.Id,
            EventVenueCalendarId = dto.EventVenueCalendarId,
            ArtistId = dto.ArtistId,
            IsHeadliner = dto.IsHeadliner,
            SetStart = dto.SetStart,
            SetEnd = dto.SetEnd
        };
    }

    public static PagedResult<ArtistPerformanceResponse> ToContract(
    PagedResultDto<ArtistPerformanceDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
