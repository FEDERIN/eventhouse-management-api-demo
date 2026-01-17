using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Artists;

public sealed class GetArtistsRequestExample
    : IExamplesProvider<GetArtistsRequest>
{
    public GetArtistsRequest GetExamples() => new()
    {
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band,
        Page = 1,
        PageSize = 20,
        SortBy = ArtistSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}
