using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.EventVenues;

public static class EventVenueMapper
{
    public static EventVenueResponse ToContract(EventVenueDto dto) => new()
    {
        Id = dto.Id,
        EventId = dto.EventId,
        VenueId = dto.VenueId,
        Status = EventVenueStatusMapper.ToContractRequired(dto.Status),
        EventName = dto.EventName,
        VenueName = dto.VenueName
    };

    public static PagedResult<EventVenueResponse> ToContract(
    PagedResultDto<EventVenueDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}