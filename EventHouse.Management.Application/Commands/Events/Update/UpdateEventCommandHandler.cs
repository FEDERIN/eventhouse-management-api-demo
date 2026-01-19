using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers;
using MediatR;

namespace EventHouse.Management.Application.Commands.Events.Update;


internal sealed class UpdateEventCommandHandler(IEventRepository eventRepository)
    : IRequestHandler<UpdateEventCommand, UpdateResult>
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<UpdateResult> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return UpdateResult.NotFound;

        entity.Update(request.Name, request.Description, EventScopeMapper.ToDomainRequired(request.Scope));

        await _eventRepository.UpdateAsync(entity, cancellationToken);

        return UpdateResult.Success;
    }
}