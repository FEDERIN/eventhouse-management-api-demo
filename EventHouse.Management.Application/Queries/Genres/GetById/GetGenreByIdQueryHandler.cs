using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.Genres.GetById;

internal sealed class GetGenreByIdQueryHandler(IGenreRepository repository)
            : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    private readonly IGenreRepository _repository = repository;

    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException("Genre", request.Id);
        
        return new GenreDto() { Id = entity.Id, Name = entity.Name };
    }
}
