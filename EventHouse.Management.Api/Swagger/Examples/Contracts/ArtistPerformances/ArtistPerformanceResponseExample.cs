using EventHouse.Management.Api.Contracts.ArtistPerformances;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.ArtistPerformances;

[ExcludeFromCodeCoverage]
internal sealed class ArtistPerformanceResponseExample : IExamplesProvider<ArtistPerformanceResponse>
{
    public ArtistPerformanceResponse GetExamples() => ArtistPerformanceExampleData.Result();
}