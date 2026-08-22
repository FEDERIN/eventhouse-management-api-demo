using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Update;

internal sealed class UpdateSeatingMapCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingMapCommand>
{
    public async Task Handle(
        UpdateSeatingMapCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = await repository.GetTrackedByIdAsync(
                    request.Id,
                    ct)
                    ?? throw new NotFoundException(
                        "SeatingMap",
                        request.Id);

                entity.Update(
                    request.Name,
                    request.Version,
                    request.IsActive);

                await repository.UpdateAsync(entity, ct);
            },
            ct);
    }
}