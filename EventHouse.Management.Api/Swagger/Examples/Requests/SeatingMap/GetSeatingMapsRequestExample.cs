using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.SeatingMap;

[ExcludeFromCodeCoverage]
internal sealed class GetSeatingMapsRequestExample : IExamplesProvider<GetSeatingMapsRequest>
{
    public GetSeatingMapsRequest GetExamples() => SeatingMapExampleData.Get();
}
