using EventHouse.Management.Api.Contracts.Seating.Structure;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Maps;

[ExcludeFromCodeCoverage]
internal sealed class SeatingMapStructureResponseExample
    : IExamplesProvider<SeatingMapStructureResponse>
{
    public SeatingMapStructureResponse GetExamples()
        => SeatingMapStructureExampleData.Result();
    
}