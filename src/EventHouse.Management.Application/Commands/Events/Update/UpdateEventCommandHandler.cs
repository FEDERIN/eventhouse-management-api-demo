using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers.Events;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

internal sealed class UpdateEventCommandHandler(
    IEventRepository eventRepository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateEventCommand, UpdateResult>
{
    public async Task<UpdateResult> Handle(
        UpdateEventCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(async ct =>
        {
            var entity = await eventRepository.GetTrackedByIdAsync(request.Id, ct)
                ?? throw new NotFoundException("Event", request.Id);

            entity.Update(
                request.Name,
                request.Description,
                EventScopeMapper.ToDomainRequired(request.Scope));

            await eventRepository.UpdateAsync(entity, ct);

        }, ct);

        return UpdateResult.Success;
    }
}