using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Genres;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Create;

internal sealed class CreateGenreCommandHandler(
    IGenreRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateGenreCommand, GenreDto>
{
    public Task<GenreDto> Handle(
        CreateGenreCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = GenreMapper.ToEntity(request);

                await repository.AddAsync(entity, ct);

                return GenreMapper.ToDto(entity);
            },
            ct);
    }
}