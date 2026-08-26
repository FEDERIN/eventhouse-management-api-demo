using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Sections;

[ExcludeFromCodeCoverage]
public sealed class AddSeatingSectionRequestExample
    : IExamplesProvider<AddSeatingSectionRequest>
{
    public AddSeatingSectionRequest GetExamples()
        => SeatingSectionExampleData.Add();
}
