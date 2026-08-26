using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Seats.UpdateStatus;

internal sealed class UpdateSeatingSeatStatusCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingSeatStatusCommand>
{
    public async Task Handle(
        UpdateSeatingSeatStatusCommand request,
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
                    seatingMap.ActivateSeat(
                        request.SeatingSectionId,
                        request.SeatingRowId,
                        request.SeatId);
                }
                else
                {
                    seatingMap.DeactivateSeat(
                        request.SeatingSectionId,
                        request.SeatingRowId,
                        request.SeatId);
                }

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}