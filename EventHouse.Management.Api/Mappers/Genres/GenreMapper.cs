using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Genres
{
    public class GenreMapper
    {
        public static GenreResponse ToContract(GenreDto dto)
        {
            return new GenreResponse
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
        public static PagedResult<GenreResponse> ToContract(
        PagedResultDto<GenreDto> paged, HttpRequest request)
        => paged.ToContract(ToContract, request);
    }
}
