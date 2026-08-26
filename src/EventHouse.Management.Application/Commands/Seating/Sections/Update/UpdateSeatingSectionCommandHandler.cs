using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.Update;

internal sealed class UpdateSeatingSectionCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingSectionCommand>
{
    public async Task Handle(
        UpdateSeatingSectionCommand request,
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

                seatingMap.UpdateSection(
                    request.SectionId,
                    request.Name,
                    request.Capacity);

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}