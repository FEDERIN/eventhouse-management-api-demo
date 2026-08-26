using EventHouse.Management.Api.Contracts.Seating.Sections;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Seating.Sections;

[ExcludeFromCodeCoverage]
public sealed class UpdateSeatingSectionRequestExample
    : IExamplesProvider<UpdateSeatingSectionRequest>
{
    public UpdateSeatingSectionRequest GetExamples()
        => SeatingSectionExampleData.Update();
}