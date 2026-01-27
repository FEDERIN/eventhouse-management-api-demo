using EventHouse.Management.Api.Contracts.Artists;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.Artists;

[ExcludeFromCodeCoverage]
internal sealed class ArtistDetailResponseExample : IExamplesProvider<ArtistDetail>
{
    public ArtistDetail GetExamples() => new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "The Rolling Stones",
        Category = ArtistCategory.Band,
        Genres =
        [
            new()
            {
                GenreId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                IsPrimary = true,
                Status = ArtistGenreStatus.Active
            },
            new()
            {
                GenreId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                IsPrimary = false,
                Status = ArtistGenreStatus.Active
            }
        ]
    };
}
