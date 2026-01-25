using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

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
