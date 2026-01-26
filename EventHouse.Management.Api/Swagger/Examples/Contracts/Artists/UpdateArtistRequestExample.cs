using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

internal sealed class UpdateArtistRequestExample
    : IExamplesProvider<UpdateArtistRequest>
{
    public UpdateArtistRequest GetExamples() => new()
    {
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band
    };
}
