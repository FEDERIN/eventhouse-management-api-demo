using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

public sealed class CreateArtistRequestExample : IExamplesProvider<CreateArtistRequest>
{
    public CreateArtistRequest GetExamples() => new()
    {
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band
    };
}
