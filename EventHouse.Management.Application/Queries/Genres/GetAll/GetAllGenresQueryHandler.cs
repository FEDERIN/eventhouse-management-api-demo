using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetAll;

internal sealed class GetAllGenresQueryHandler(IGenreRepository genreRepository)
            : IRequestHandler<GetAllGenresQuery, PagedResultDto<GenreDto>>
{
    private readonly IGenreRepository _genreRepository = genreRepository;

    public async Task<PagedResultDto<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var criteria = new GenreQueryCriteria
        {
            Name = request.Name,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection                
        };

        var result = await _genreRepository.GetPagedAsync(
            criteria,
            cancellationToken
        );

        var dtoList = result.Items.Select(e => new GenreDto
        {
            Id = e.Id,
            Name = e.Name
        }).ToList();

        return new PagedResultDto<GenreDto>
        {
            Items = dtoList,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
