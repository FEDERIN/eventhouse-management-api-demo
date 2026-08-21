using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Genres;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Create;

internal sealed class CreateGenreCommandHandler(IGenreRepository genreRepository) : IRequestHandler<CreateGenreCommand, GenreDto>
{
    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken ct)
    {
        var entity = GenreMapper.ToEntity(request);

        await genreRepository.AddAsync(entity, ct);
        return GenreMapper.ToDto(entity);
    }
}