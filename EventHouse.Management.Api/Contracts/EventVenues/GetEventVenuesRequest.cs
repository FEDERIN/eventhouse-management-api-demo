using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace EventHouse.Management.Api.Contracts.EventVenues;

public sealed record GetEventVenuesRequest : SortablePaginationRequest<EventVenueSortBy>
{
    [FromQuery(Name = "eventId")]
    public Guid? EventId { get; init; }
    [FromQuery(Name = "venueId")]
    public Guid? VenueId { get; init; }
    [FromQuery(Name = "status")]
    public EventVenueStatus? Status { get; init; }
}
