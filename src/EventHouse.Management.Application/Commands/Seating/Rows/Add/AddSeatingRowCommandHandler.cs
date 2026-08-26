using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Rows.Add;

internal sealed class AddSeatingRowCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<AddSeatingRowCommand>
{
    public async Task Handle(
        AddSeatingRowCommand request,
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

                seatingMap.AddRow(
                    request.SeatingSectionId,
                    request.Number,
                    request.Label);

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}