using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Genres;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetById;

internal sealed class GetGenreByIdQueryHandler(IGenreRepository repository)
            : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct) 
            ?? throw new NotFoundException("Genre", request.Id);
        
        return GenreMapper.ToDto(entity);
    }
}