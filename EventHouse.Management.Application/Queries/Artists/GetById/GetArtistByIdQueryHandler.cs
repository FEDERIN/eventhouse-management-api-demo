using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Queries.Artists.GetById;

internal sealed class GetArtistByIdQueryHandler(IArtistRepository repository)
        : IRequestHandler<GetArtistByIdQuery, ArtistDto>
{
    private readonly IArtistRepository _repository = repository;

    public async Task<ArtistDto> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Artist", request.Id);

        return ArtistMapper.ToDto(entity);
    }
}
