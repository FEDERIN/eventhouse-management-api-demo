using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Genres;
using EventHouse.Management.Domain.Entities;
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
        return GenreMapper.ToDto(entity);
    }
}
