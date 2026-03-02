using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Events;


[ExcludeFromCodeCoverage]
internal sealed class GetEventsRequestExample
    : IExamplesProvider<GetEventsRequest>
{
    public GetEventsRequest GetExamples() => new()
    {
        Name = "Annual Conference",
        Description = "A conference held annually to discuss industry trends.",
        Scope = EventScope.National,
        Page = 1,
        PageSize = 10,
        SortBy = EventSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}
