using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class CreateArtistRequestExample : IExamplesProvider<CreateArtistRequest>
{
    public CreateArtistRequest GetExamples() => new()
    {
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band
    };
}
