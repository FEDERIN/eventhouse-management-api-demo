
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.Events
{
    internal class EventsMapper
    {
        public static EventDto ToDto(Event entity)
        {
            return new EventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Scope = EventScopeMapper.ToApplicationRequired(entity.Scope)
            };
        }

        public static IReadOnlyList<EventDto> ToDto(IReadOnlyList<Event> entities)
        {
            return [.. entities.Select(ToDto)];
        }
    }
}
