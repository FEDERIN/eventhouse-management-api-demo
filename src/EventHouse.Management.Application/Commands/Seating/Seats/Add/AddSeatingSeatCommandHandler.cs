using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Seats.Add;

internal sealed class AddSeatingSeatCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<AddSeatingSeatCommand>
{
    public async Task Handle(
        AddSeatingSeatCommand request,
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

                seatingMap.AddSeat(
                    request.SeatingSectionId,
                    request.SeatingRowId,
                    request.Number,
                    request.Label);

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}