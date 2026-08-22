using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Create;

internal sealed class CreateArtistCommandHandler(
    IArtistRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateArtistCommand, ArtistDto>
{
    public Task<ArtistDto> Handle(
        CreateArtistCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = ArtistMapper.ToEntity(request);

                await repository.AddAsync(entity, ct);

                return ArtistMapper.ToDto(entity);
            },
            ct);
    }
}