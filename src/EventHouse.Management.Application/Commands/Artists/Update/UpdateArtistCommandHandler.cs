using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update;

internal sealed class UpdateArtistCommandHandler(
    IArtistRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateArtistCommand>
{
    public async Task Handle(
        UpdateArtistCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = await repository.GetTrackedByIdAsync(
                    request.Id,
                    ct)
                    ?? throw new NotFoundException(
                        "Artist",
                        request.Id);

                entity.Update(
                    request.Name,
                    ArtistCategoryMapper.ToDomainRequired(request.Category));

                await repository.UpdateAsync(entity, ct);
            },
            ct);
    }
}