using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Queries.Artists.GetAll;

namespace EventHouse.Management.Api.Mappers.Artists;
public static class GetAllArtistsQueryMapper 
{
    public static GetAllArtistsQuery FromContract(GetArtistsRequest request)
        => new()
        {
            Name = request.Name,
            Category = ArtistCategoryMapper.ToApplicationOptional(request.Category),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = ArtistSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}