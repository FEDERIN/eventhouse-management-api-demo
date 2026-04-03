using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class ArtistPagedResultExample : IExamplesProvider<PagedResult<ArtistSummary>>
{
    public PagedResult<ArtistSummary> GetExamples() => new()
    {
        Items =
        [ArtistExampleData.ResultSumary()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,

        Links = new PaginationLinks
        {
            Self = "/api/v1/artists?page=1&pageSize=20",
            First = "/api/v1/artists?page=1&pageSize=20",
            Last = "/api/v1/artists?page=1&pageSize=20"
        }
    };
}
