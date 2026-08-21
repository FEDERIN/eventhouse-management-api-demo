using MediatR;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Events;

namespace EventHouse.Management.Application.Commands.Events.Create;

internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateEventCommand, EventDto>
{
    public Task<EventDto> Handle(
        CreateEventCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = EventsMapper.ToEntity(request);

                await eventRepository.AddAsync(entity, ct);

                return EventsMapper.ToDto(entity);
            },
            ct);
    }
}