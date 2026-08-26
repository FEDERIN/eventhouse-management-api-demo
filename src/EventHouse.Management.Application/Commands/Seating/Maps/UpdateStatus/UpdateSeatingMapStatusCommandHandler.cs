using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Maps.UpdateStatus;

internal sealed class UpdateSeatingMapStatusCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingMapStatusCommand>
{
    public async Task Handle(
        UpdateSeatingMapStatusCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var seatingMap =
                    await repository.GetTrackedWithStructureByIdAsync(
                        request.SeatingMapId,
                        ct)
                    ?? throw new NotFoundException(
                        "SeatingMap",
                        request.SeatingMapId);

                if (request.IsActive)
                {
                    seatingMap.Activate();
                }
                else
                {
                    seatingMap.Deactivate();
                }

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}