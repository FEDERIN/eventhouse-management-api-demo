using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class ArtistSumaryResponseExample : IExamplesProvider<ArtistSummary>
{
    public ArtistSummary GetExamples() => ArtistExampleData.ResultSumary();
}
