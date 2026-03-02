using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Genres;

[ExcludeFromCodeCoverage]
internal sealed class GetGenresRequestExample : IExamplesProvider<GetGenresRequest>
{
    public GetGenresRequest GetExamples() => new()
    {
        Name = "Rock",
        Page = 1,
        PageSize = 15,
        SortBy = GenreSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}