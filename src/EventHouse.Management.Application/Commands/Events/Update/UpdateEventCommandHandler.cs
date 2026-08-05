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
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IApplicationResilience _resilience = resilience;

    public async Task<UpdateResult> Handle(
        UpdateEventCommand request,
        CancellationToken cancellationToken)
    {
        await _resilience.ExecuteSqlAsync(async ct =>
        {
            var entity = await _eventRepository.GetTrackedByIdAsync(request.Id, ct)
                ?? throw new NotFoundException("Event", request.Id);

            entity.Update(
                request.Name,
                request.Description,
                EventScopeMapper.ToDomainRequired(request.Scope));

            await _eventRepository.UpdateAsync(entity, ct);

        }, cancellationToken);

        return UpdateResult.Success;
    }
}