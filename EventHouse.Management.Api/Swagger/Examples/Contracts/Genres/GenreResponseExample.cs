using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

[ExcludeFromCodeCoverage]
internal class GenreResponseExample : IExamplesProvider<GenreResponse>
{
    public GenreResponse GetExamples() => new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "Rock"
    };
}
