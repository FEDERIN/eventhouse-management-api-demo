using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Add;

internal sealed class AddSeatingSectionCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<AddSeatingSectionCommand>
{
    public async Task Handle(
        AddSeatingSectionCommand request,
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

                seatingMap.AddSection(
                    request.Name,
                    request.IsNumbered,
                    request.Capacity);

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}