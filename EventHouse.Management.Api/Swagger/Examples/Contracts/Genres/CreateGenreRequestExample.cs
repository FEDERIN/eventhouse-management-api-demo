using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

internal sealed class CreateGenreRequestExample : IExamplesProvider<CreateGenreRequest>
{
    public CreateGenreRequest GetExamples() => new()
    {
        Name = "Rock"
    };
}
