using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Events;
using MediatR;

internal sealed class UpdateEventCommandHandler(
    IEventRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateEventCommand>
{
    public async Task Handle(
        UpdateEventCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(async ct =>
        {
            var entity = await repository.GetTrackedByIdAsync(request.Id, ct)
                ?? throw new NotFoundException("Event", request.Id);

            entity.Update(
                request.Name,
                request.Description,
                EventScopeMapper.ToDomainRequired(request.Scope));

            await repository.UpdateAsync(entity, ct);

        }, ct);
    }
}