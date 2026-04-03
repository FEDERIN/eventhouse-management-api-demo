using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

[ExcludeFromCodeCoverage]
internal class GenrePagedResultExample : IExamplesProvider<PagedResult<GenreResponse>>
{
    public PagedResult<GenreResponse> GetExamples() => new()
    {
        Items = [GenreExampleData.Result()],
        TotalCount = 1,
        Page = 1,
        PageSize = 20,
        Links = new PaginationLinks
        {
            Self = "/api/v1/genres?page=1&pageSize=20",
            First = "/api/v1/genres?page=1&pageSize=20",
            Last = "/api/v1/genres?page=1&pageSize=20"
        }
    };
}
