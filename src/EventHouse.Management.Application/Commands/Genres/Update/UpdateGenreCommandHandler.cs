using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update;

internal sealed class UpdateGenreCommandHandler(
    IGenreRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateGenreCommand>
{
    public async Task Handle(
        UpdateGenreCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = await repository.GetTrackedByIdAsync(
                    request.Id,
                    ct)
                    ?? throw new NotFoundException(
                        "Genre",
                        request.Id);

                entity.Update(request.Name);

                await repository.UpdateAsync(entity, ct);
            },
            ct);
    }
}