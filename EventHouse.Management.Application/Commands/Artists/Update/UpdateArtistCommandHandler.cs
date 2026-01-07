using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update;

internal sealed class UpdateArtistCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<UpdateArtistCommand, UpdateResult>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task<UpdateResult> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _artistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return UpdateResult.NotFound;

        try
        {
            entity.Update(request.Name, ArtistCategoryMapper.ToDomainRequired(request.Category));

            await _artistRepository.UpdateAsync(entity, cancellationToken);

            return UpdateResult.Success;
        }
        catch (ArgumentException)
        {
            return UpdateResult.ValidationFailed;
        }
        catch (InvalidOperationException)
        {
            return UpdateResult.Conflict;
        }
    }
}
