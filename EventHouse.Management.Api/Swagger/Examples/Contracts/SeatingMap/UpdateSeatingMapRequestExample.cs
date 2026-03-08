using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

[ExcludeFromCodeCoverage]
internal class UpdateSeatingMapRequestExample : IExamplesProvider<UpdateSeatingMapRequest>
{
    public UpdateSeatingMapRequest GetExamples() => SeatingMapExampleData.Update();
}
