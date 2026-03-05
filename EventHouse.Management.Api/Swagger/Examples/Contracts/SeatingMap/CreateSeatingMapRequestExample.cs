using EventHouse.Management.Api.Contracts.SeatingMaps;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.SeatingMap;

[ExcludeFromCodeCoverage]
internal sealed class CreateSeatingMapRequestExample : IExamplesProvider<CreateSeatingMapRequest>
{
    public CreateSeatingMapRequest GetExamples() => SeatingMapExampleData.Create();
}
