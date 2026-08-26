using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Sections.UpdateStatus;

internal sealed class UpdateSeatingSectionStatusCommandHandler(
    ISeatingMapRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateSeatingSectionStatusCommand>
{
    public async Task Handle(
        UpdateSeatingSectionStatusCommand request,
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
                    seatingMap.ActivateSection(
                        request.SectionId);
                }
                else
                {
                    seatingMap.DeactivateSection(
                        request.SectionId);
                }

                await repository.UpdateAsync(
                    seatingMap,
                    ct);
            },
            ct);
    }
}