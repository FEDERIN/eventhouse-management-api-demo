using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenueCalendars;

[ExcludeFromCodeCoverage]
internal sealed class EventVenueCalendarPagedResultExample : IExamplesProvider<PagedResult<EventVenueCalendarResponse>>
{
    public PagedResult<EventVenueCalendarResponse> GetExamples() => new()
    {
        Items = [EventVenueCalendarExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/event-venue-calendars?page=1&pageSize=20",
            First = "/api/v1/event-venue-calendars?page=1&pageSize=20",
            Last = "/api/v1/event-venue-calendars?page=1&pageSize=20"
        }
    };
}
