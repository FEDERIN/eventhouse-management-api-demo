using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.EventVenues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenues;

[ExcludeFromCodeCoverage]
internal sealed class EventVenuePagedResultExample : IExamplesProvider<PagedResult<EventVenueResponse>>
{
    public PagedResult<EventVenueResponse> GetExamples() => new()
    {
        Items = [EventVenueExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/event-venues?page=1&pageSize=20",
            First = "/api/v1/event-venues?page=1&pageSize=20",
            Last = "/api/v1/event-venues?page=1&pageSize=20"
        }
    };
}
