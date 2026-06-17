using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

[ExcludeFromCodeCoverage]
internal sealed class SeatingMapResponseExample : IExamplesProvider<SeatingMapResponse>
{
    public SeatingMapResponse GetExamples() => SeatingMapExampleData.Result();
}
