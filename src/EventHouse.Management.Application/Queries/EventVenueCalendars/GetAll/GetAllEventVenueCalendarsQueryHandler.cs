using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

internal sealed class GetAllEventVenueCalendarsQueryHandler(IEventVenueCalendarRepository repository) : IRequestHandler<GetAllEventVenueCalendarsQuery, PagedResultDto<EventVenueCalendarDto>>
{
    public async Task<PagedResultDto<EventVenueCalendarDto>> Handle(GetAllEventVenueCalendarsQuery request, CancellationToken ct)
    {
        var criteria = new EventVenueCalendarQueryCriteria
        {
            EventVenueId = request.EventVenueId,
            SeatingMapId = request.SeatingMapId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status =  EventVenueCalendarStatusMapper.ToDomainOptional(request.Status),
            TimeZoneId = request.TimeZoneId,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
        };

        var result = await repository.GetPagedAsync(criteria, ct);

        return result.MapTo(EventVenueCalendarMapper.ToDto);
    }
}