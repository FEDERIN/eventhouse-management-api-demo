using EventHouse.Management.Api.Contracts.Genres;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

[ExcludeFromCodeCoverage]
internal sealed class UpdateGenreRequestExample : IExamplesProvider<UpdateGenreRequest>
{
    public UpdateGenreRequest GetExamples() => new()
    {
        Name = "Rock and Roll"
    };
}
