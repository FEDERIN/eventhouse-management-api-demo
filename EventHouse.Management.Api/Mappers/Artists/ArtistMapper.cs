using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Artists;

public class ArtistMapper
{
   public static ArtistDetail ToContract(ArtistDto dto)
    {
        return new ArtistDetail
        {
            Id = dto.Id,
            Name = dto.Name,
            Category = ArtistCategoryMapper.ToContract(dto.Category),
            Genres = [.. dto.Genres.Select(g => new ArtistGenre
            {
                GenreId = g.GenreId,
                Status = ArtistGenreStatusMapper.ToContract(g.Status),
                IsPrimary = g.IsPrimary
            })]
        };
    }
    public static ArtistSummary ToContractSumary(ArtistDto dto)
    {
        return new ArtistSummary
        {
            Id = dto.Id,
            Name = dto.Name,
            Category = ArtistCategoryMapper.ToContract(dto.Category)
        };
    }

    public static PagedResult<ArtistSummary> ToContract(
    PagedResultDto<ArtistDto> paged, HttpRequest request)
    => paged.ToContract(ToContractSumary, request);
}