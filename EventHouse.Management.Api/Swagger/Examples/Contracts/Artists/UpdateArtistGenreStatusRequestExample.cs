using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class UpdateArtistGenreStatusRequestExample
    : IExamplesProvider<UpdateArtistGenreStatusRequest>
{
    public UpdateArtistGenreStatusRequest GetExamples()
    {
        return new UpdateArtistGenreStatusRequest
        {
            Status = ArtistGenreStatus.Active
        };
    }
}
