using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Create;

internal sealed class CreateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IGenreRepository _genreRepository = genreRepository;

    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = new Genre(
            Guid.NewGuid(),
            request.Name
        );

        await _genreRepository.AddAsync(entity, cancellationToken);
        return new GenreDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}
