using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class ArtistPagedResultExample : IExamplesProvider<PagedResult<ArtistSummary>>
{
    public PagedResult<ArtistSummary> GetExamples() => new()
    {
        Items =
        [
            new() {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "The Rolling Stones",
                Category = ArtistCategory.Band
            }
        ],
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
