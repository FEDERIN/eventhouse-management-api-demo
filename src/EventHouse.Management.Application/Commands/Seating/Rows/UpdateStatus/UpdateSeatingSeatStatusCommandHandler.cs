using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Rows.UpdateStatus;

internal sealed class UpdateSeatingRowStatusCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingRowStatusCommand>
{
    public async Task Handle(
        UpdateSeatingRowStatusCommand request,
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
                    seatingMap.ActivateRow(
                        request.SeatingSectionId,
                        request.RowId);
                }
                else
                {
                    seatingMap.DeactivateRow(
                        request.SeatingSectionId,
                        request.RowId);
                }

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}