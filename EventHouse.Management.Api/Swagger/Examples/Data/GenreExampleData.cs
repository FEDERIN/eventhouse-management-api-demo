using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class GenreExampleData
{
    internal static readonly Guid GenreId = ExampleConstants.GenreId;
    internal static readonly string Name = "Rock";

    internal static CreateGenreRequest Create() => new()
    {
        Name = Name
    };

    internal static UpdateGenreRequest Update() => new()
    {
        Name = Name
    };

    internal static GenreResponse Result() => new()
    {
        Id = GenreId,
        Name = Name
    };


    internal static GetGenresRequest Get() => new()
    {
        Name = Name,
        Page = 1,
        PageSize = 15,
        SortBy = GenreSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}
