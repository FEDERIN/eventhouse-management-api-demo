using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists
{
    internal sealed class AddArtistGenreRequestExample
        : IExamplesProvider<AddArtistGenreRequest>
    {
        public AddArtistGenreRequest GetExamples() => new()
        {
            GenreId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            IsPrimary = true,
            Status = ArtistGenreStatus.Active
        };
    }
}
