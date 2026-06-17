using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.ArtistPerformances;

[ExcludeFromCodeCoverage]
internal sealed class GetArtistPerformancesRequestExample : IExamplesProvider<GetArtistPerformancesRequest>
{
    public GetArtistPerformancesRequest GetExamples() => ArtistPerformanceExampleData.Get();
}