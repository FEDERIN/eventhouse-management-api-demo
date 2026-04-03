using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Genres;

[ExcludeFromCodeCoverage]
internal sealed class GetGenresRequestExample : IExamplesProvider<GetGenresRequest>
{
    public GetGenresRequest GetExamples() => GenreExampleData.Get();
}