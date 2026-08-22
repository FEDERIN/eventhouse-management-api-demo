using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Events;
using MediatR;

namespace EventHouse.Management.Application.Queries.Events.GetById;

internal sealed class GetEventByIdQueryHandler(IEventRepository repository)
        : IRequestHandler<GetEventByIdQuery, EventDto>
{
    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct)
        ?? throw new NotFoundException("Event", request.Id);

        return EventsMapper.ToDto(entity);
    }
}