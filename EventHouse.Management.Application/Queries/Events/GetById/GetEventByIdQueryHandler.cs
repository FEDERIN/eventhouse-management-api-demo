using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers;
using MediatR;

namespace EventHouse.Management.Application.Queries.Events.GetById;

internal sealed class GetEventByIdQueryHandler(IEventRepository repository)
        : IRequestHandler<GetEventByIdQuery, EventDto?>
{
    private readonly IEventRepository _repository = repository;

    public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return null;

        return new EventDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Scope = EventScopeMapper.ToApplicationRequired(entity.Scope)
        };
    }
}
