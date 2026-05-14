using EventHouse.Management.Api.Contracts.ArtistPerformances;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.ArtistPerformances;

[ExcludeFromCodeCoverage]
public sealed class SwapHeadlinerRequestExample : IExamplesProvider<SwapHeadlinerRequest>
{
    public SwapHeadlinerRequest GetExamples()
    {
        return new SwapHeadlinerRequest(
            Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
            Guid.Parse("z9y8x7w6-v5u4-4t3s-2r1q-0p9o8n7m6l5k")
        );
    }
}