using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Genres;

[ExcludeFromCodeCoverage]
internal sealed class UpdateGenreRequestExample : IExamplesProvider<UpdateGenreRequest>
{
    public UpdateGenreRequest GetExamples() => GenreExampleData.Update();
}
