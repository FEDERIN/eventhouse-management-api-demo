using EventHouse.Management.Api.Contracts.SeatingMaps;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

internal sealed class SeatingMapResponseExample : IExamplesProvider<SeatingMapResponse>
{
    public SeatingMapResponse GetExamples() => SeatingMapExampleData.Result();
}
