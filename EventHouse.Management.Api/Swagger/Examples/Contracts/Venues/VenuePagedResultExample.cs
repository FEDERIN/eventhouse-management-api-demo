using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Venues;

[ExcludeFromCodeCoverage]
internal class VenuePagedResultExample : IExamplesProvider<PagedResult<Venue>>
{
    public PagedResult<Venue> GetExamples() => new()
    {
        Items = [VenueExampleData.MadisonSquareGarden()],
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