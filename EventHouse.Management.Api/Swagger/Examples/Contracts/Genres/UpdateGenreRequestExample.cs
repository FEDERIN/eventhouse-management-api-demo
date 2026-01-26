using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

internal sealed class UpdateGenreRequestExample : IExamplesProvider<UpdateGenreRequest>
{
    public UpdateGenreRequest GetExamples() => new()
    {
        Name = "Rock and Roll"
    };
}
