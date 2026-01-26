using Swashbuckle.AspNetCore.Filters;
using EventHouse.Management.Api.Contracts.Artists;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

internal sealed class ArtistResponseExample : IExamplesProvider<Artist>
{
    public Artist GetExamples() => new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band
    };
}
