using EventHouse.Management.Api.Contracts.Seating.Rows;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Rows;

[ExcludeFromCodeCoverage]
internal sealed class AddSeatingRowRequestExample
    : IExamplesProvider<AddSeatingRowRequest>
{
    public AddSeatingRowRequest GetExamples() =>
        SeatingRowExampleData.Add();
}