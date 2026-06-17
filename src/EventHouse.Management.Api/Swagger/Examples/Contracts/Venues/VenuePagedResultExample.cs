using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal class VenuePagedResultExample : IExamplesProvider<PagedResult<VenueResponse>>
{
    public PagedResult<VenueResponse> GetExamples() => new()
    {
        Items = [VenueExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/venues?page=1&pageSize=20",
            First = "/api/v1/venues?page=1&pageSize=20",
            Last = "/api/v1/venues?page=1&pageSize=20"
        }
    };
}