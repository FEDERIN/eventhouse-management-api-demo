using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.ArtistPerformances;

[ExcludeFromCodeCoverage]
internal sealed class ArtistPerformancePagedResultExample : IExamplesProvider<PagedResult<ArtistPerformanceResponse>>
{
    public  PagedResult<ArtistPerformanceResponse> GetExamples()
    {
        return new()
        {
            Items = [ArtistPerformanceExampleData.Result()],
            TotalCount = 1,
            Page = 1,
            PageSize = 20,
            Links = new PaginationLinks
            {
                Self = "/api/v1/artist-performances?page=1&pageSize=20",
                First = "/api/v1/artist-performances?page=1&pageSize=20",
                Last = "/api/v1/artist-performances?page=1&pageSize=20"
            }
        };
    }
}
