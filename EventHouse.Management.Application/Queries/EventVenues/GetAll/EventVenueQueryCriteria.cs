using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Queries.EventVenues.GetAll;

public sealed record EventVenueQueryCriteria : SortablePaginationCriteria<EventVenueSortField>
{
    public Guid? EventId { get; init; }
    public Guid? VenueId { get; init; }
    public EventVenueStatus? Status { get; init; }
}