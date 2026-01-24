using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update;

internal sealed class UpdateArtistCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<UpdateArtistCommand, UpdateResult>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task<UpdateResult> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _artistRepository.GetTrackedByIdAsync(request.Id, cancellationToken)
        ?? throw new NotFoundException("Artist", request.Id);

        entity.Update(request.Name, ArtistCategoryMapper.ToDomainRequired(request.Category));

        await _artistRepository.UpdateAsync(entity, cancellationToken);

        return UpdateResult.Success;
    }
}
