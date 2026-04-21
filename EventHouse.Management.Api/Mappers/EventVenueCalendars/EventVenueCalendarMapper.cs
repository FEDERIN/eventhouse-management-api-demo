using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;


namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

public static class EventVenueCalendarMapper
{
    public static EventVenueCalendarResponse ToContract(EventVenueCalendarDto dto) => new()
    {
        Id = dto.Id,
        EventVenueId = dto.EventVenueId,
        SeatingMapId = dto.SeatingMapId,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        TimeZoneId = dto.TimeZoneId,
        Status = EventVenueCalendarStatusMapper.ToContractRequired(dto.Status)
    };

    public static PagedResult<EventVenueCalendarResponse> ToContract(
    PagedResultDto<EventVenueCalendarDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
