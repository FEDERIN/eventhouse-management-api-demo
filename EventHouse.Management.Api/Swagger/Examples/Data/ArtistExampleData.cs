using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class ArtistExampleData
{
    private static readonly Guid ArtistId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851");
    private static readonly string ArtistName = "The Rolling Stones";
    private static readonly ArtistCategory ArtistCategory = ArtistCategory.Band;
    private static readonly Guid GenreId = ExampleConstants.GenreId;
    private static readonly ArtistGenreStatus ArtistGenreStatus = ArtistGenreStatus.Active;

    internal static CreateArtistRequest Create() => new()
    {
        Name = ArtistName,
        Category = ArtistCategory
    };
    
    internal static UpdateArtistRequest Update() => new()
    {
        Name = ArtistName,
        Category = ArtistCategory
    };
    
    internal static UpdateArtistGenreStatusRequest UpdateGenreStatus() => new()
    {
        Status = ArtistGenreStatus
    };
    
    internal static ArtistDetail Result() => new()
    {
        Id = ArtistId,
        Name = ArtistName,
        Category = ArtistCategory,
        Genres =
        [
            new() {
                GenreId = GenreId,
                Status = ArtistGenreStatus
            }
        ]
    };

    internal static ArtistSummary ResultSumary() => new()
    {
        Id = ArtistId,
        Name = ArtistName,
        Category = ArtistCategory
    };

    internal static AddArtistGenreRequest AddArtistGenre() => new()
    {
        GenreId = GenreId,
        IsPrimary = true,
        Status = ArtistGenreStatus
    };

    internal static GetArtistsRequest Get()
    {
        return new()
        {
            Name = ArtistName,
            Category = ArtistCategory,
            Page = 1,
            PageSize = 20,
            SortBy = ArtistSortBy.Name,
            SortDirection = SortDirection.Asc
        };
    }
}
