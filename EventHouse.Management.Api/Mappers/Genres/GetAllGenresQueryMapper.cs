using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Application.Queries.Genres.GetAll;

namespace EventHouse.Management.Api.Mappers.Genres;

public static class GetAllGenresQueryMapper
{
    public static GetAllGenresQuery FromContract(GetGenresRequest request)
        => new()
        {
            Name = request.Name,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = GenreSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}
