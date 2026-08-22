using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Genres;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetAll;

internal sealed class GetAllGenresQueryHandler(IGenreRepository genreRepository)
            : IRequestHandler<GetAllGenresQuery, PagedResultDto<GenreDto>>
{
    public async Task<PagedResultDto<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken ct)
    {
        var criteria = new GenreQueryCriteria
        {
            Name = request.Name,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection                
        };

        var result = await genreRepository.GetPagedAsync(
            criteria,
            ct
        );

        return result.MapTo(GenreMapper.ToDto);
    }
}