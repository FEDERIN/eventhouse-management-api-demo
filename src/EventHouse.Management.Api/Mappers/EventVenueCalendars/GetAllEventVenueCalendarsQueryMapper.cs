using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;

namespace EventHouse.Management.Api.Mappers.EventVenueCalendars;

internal static class GetAllEventVenueCalendarsQueryMapper
{
    public static GetAllEventVenueCalendarsQuery FromContract(GetEventVenueCalendarsRequest request)
        => new()
        {
            EventVenueId = request.EventVenueId,
            SeatingMapId = request.SeatingMapId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TimeZoneId = request.TimeZoneId,
            Status = EventVenueCalendarStatusMapper.ToApplicationOptional(request.Status),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = EventVenueCalendarSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}
