using MediatR;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers;

namespace EventHouse.Management.Application.Commands.Events.Create;

internal sealed class CreateEventCommandHandler(IEventRepository eventRepository) : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = new Event(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            EventScopeMapper.ToDomainRequired(request.Scope)
        );

        await _eventRepository.AddAsync(entity, cancellationToken);

        return new EventDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Scope = EventScopeMapper.ToApplicationRequired(entity.Scope)
        }; 
    }
}
