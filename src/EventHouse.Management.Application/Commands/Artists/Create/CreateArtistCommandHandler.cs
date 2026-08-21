using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Create;

internal sealed class CreateArtistCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<CreateArtistCommand, ArtistDto>
{
    public async Task<ArtistDto> Handle(CreateArtistCommand request, CancellationToken ct)
    {
        var entity = ArtistMapper.ToEntity(request);

        await artistRepository.AddAsync(entity, ct);

        return ArtistMapper.ToDto(entity);
    }
}