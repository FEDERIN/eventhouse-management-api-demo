using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class ArtistDetailResponseExample : IExamplesProvider<ArtistDetail>
{
    public ArtistDetail GetExamples() => ArtistExampleData.Result();
}
