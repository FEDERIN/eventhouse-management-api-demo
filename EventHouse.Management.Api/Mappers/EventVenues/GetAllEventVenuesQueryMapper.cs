using EventHouse.Management.Api.Contracts.EventVenues;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;

namespace EventHouse.Management.Api.Mappers.EventVenues;

public static class GetAllEventVenuesQueryMapper
{
    public static GetAllEventVenuesQuery FromContract(GetEventVenuesRequest request)
        => new()
        {
            EventId = request.EventId,
            VenueId = request.VenueId,
            Status = EventVenueStatusMapper.ToApplicationOptional(request.Status),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = EventVenueSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}
