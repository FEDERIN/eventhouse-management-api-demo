using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Artists;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.Artists.GetById;

internal sealed class GetArtistByIdQueryHandler(IArtistRepository repository)
        : IRequestHandler<GetArtistByIdQuery, ArtistDto>
{
    public async Task<ArtistDto> Handle(GetArtistByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Artist", request.Id);

        return ArtistMapper.ToDtoWithRelation(entity);
    }
}