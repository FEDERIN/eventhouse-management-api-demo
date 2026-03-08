using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Artists;

[ExcludeFromCodeCoverage]
internal sealed class GetArtistsRequestExample
    : IExamplesProvider<GetArtistsRequest>
{
    public GetArtistsRequest GetExamples() => ArtistExampleData.Get();
}
