using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Genres
{
    public class GenreMapper
    {
        public static Genre ToContract(GenreDto dto)
        {
            return new Genre
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
        public static PagedResult<Genre> ToContract(
        PagedResultDto<GenreDto> paged, HttpRequest request)
        => paged.ToContract(ToContract, request);
    }
}
