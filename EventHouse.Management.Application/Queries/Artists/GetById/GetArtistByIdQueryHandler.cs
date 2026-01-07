using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;
using EventHouse.Management.Application.Mappers;

namespace EventHouse.Management.Application.Queries.Artists.GetById;

internal sealed class GetArtistByIdQueryHandler(IArtistRepository repository)
        : IRequestHandler<GetArtistByIdQuery, ArtistDto?>
{
    private readonly IArtistRepository _repository = repository;

    public async Task<ArtistDto?> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return null;

        return new ArtistDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Category = ArtistCategoryMapper.ToApplication(entity.Category),
        };
    }
}
