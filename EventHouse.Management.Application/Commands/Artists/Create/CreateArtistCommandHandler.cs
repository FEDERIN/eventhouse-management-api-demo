using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Create;

internal sealed class CreateArtistCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<CreateArtistCommand, ArtistDto>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task<ArtistDto> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = new Artist(
            id: Guid.NewGuid(),
            name: request.Name.Trim(),
            category: ArtistCategoryMapper.ToDomainRequired(request.Category));

        await _artistRepository.AddAsync(entity, cancellationToken);

        return new ArtistDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Category = ArtistCategoryMapper.ToApplication(entity.Category)
        };
    }
}
