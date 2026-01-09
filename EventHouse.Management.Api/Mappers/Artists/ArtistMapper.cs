using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Artists;

public class ArtistMapper
{
   public static Artist ToContract(ArtistDto dto)
    {
        return new Artist
        {
            Id = dto.Id,
            Name = dto.Name,
            Category = ArtistCategoryMapper.ToContract(dto.Category)
        };
    }

    public static PagedResult<Artist> ToContract(
    PagedResultDto<ArtistDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
