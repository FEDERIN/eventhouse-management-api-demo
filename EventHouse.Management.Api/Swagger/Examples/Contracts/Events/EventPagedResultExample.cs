using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class EventPagedResultExample : IExamplesProvider<PagedResult<EventResponse>>
{
    public PagedResult<EventResponse> GetExamples() => new()
    {
        Items =
        [EventExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/events?page=1&pageSize=20",
            First = "/api/v1/events?page=1&pageSize=20",
            Last = "/api/v1/events?page=1&pageSize=20"
        }
    };
}
