using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

internal class GenrePagedResultExample : IExamplesProvider<PagedResult<Genre>>
{
    public PagedResult<Genre> GetExamples() => new()
    {
        Items =
        [
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Rock"
            }
        ],
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
