using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

public sealed record EventVenueCalendarQueryCriteria : SortablePaginationCriteria<EventVenueCalendarSortField>
{
    public Guid? EventVenueId { get; init; }
    public Guid? SeatingMapId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? TimeZoneId { get; init; }
    public EventVenueCalendarStatus? Status { get; init; }
}
