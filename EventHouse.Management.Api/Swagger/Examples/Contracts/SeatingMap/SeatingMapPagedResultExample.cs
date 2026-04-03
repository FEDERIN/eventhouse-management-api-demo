using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

[ExcludeFromCodeCoverage]
internal sealed class SeatingMapPagedResultExample : IExamplesProvider<PagedResult<SeatingMapResponse>>
{
    public PagedResult<SeatingMapResponse> GetExamples() => new()
    {
        Items = [SeatingMapExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/seatingMaps?page=1&pageSize=20",
            First = "/api/v1/seatingMaps?page=1&pageSize=20",
            Last = "/api/v1/seatingMaps?page=1&pageSize=20"
        }
    };
}
