using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers.Artists;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update;

internal sealed class UpdateArtistCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<UpdateArtistCommand, UpdateResult>
{
    public async Task<UpdateResult> Handle(UpdateArtistCommand request, CancellationToken ct)
    {
        var entity = await artistRepository.GetTrackedByIdAsync(request.Id, ct)
        ?? throw new NotFoundException("Artist", request.Id);

        entity.Update(request.Name, ArtistCategoryMapper.ToDomainRequired(request.Category));

        await artistRepository.UpdateAsync(entity, ct);

        return UpdateResult.Success;
    }
}