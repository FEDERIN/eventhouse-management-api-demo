using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Events;

[ExcludeFromCodeCoverage]
internal sealed class EventPagedResultExample : IExamplesProvider<PagedResult<Event>>
{
    public PagedResult<Event> GetExamples() => new()
    {
        Items =
        [
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Summer Music Festival",
                Description = "An exciting outdoor music festival featuring various artists.",
                Scope = EventScope.National
            }
        ],
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
