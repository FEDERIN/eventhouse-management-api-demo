using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

[ExcludeFromCodeCoverage]
internal sealed class CreateGenreRequestExample : IExamplesProvider<CreateGenreRequest>
{
    public CreateGenreRequest GetExamples() => new()
    {
        Name = "Rock"
    };
}
