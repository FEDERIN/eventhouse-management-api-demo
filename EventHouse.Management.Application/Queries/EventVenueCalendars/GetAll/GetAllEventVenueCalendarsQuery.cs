using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;


public sealed record GetAllEventVenueCalendarsQuery
    : SortablePaginationQuery<EventVenueCalendarSortField>, IRequest<PagedResultDto<EventVenueCalendarDto>>
{
    public Guid? EventVenueId { get; init; }
    public Guid? SeatingMapId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? TimeZoneId { get; init; }
    public EventVenueCalendarStatusDto? Status { get; init; }
}
