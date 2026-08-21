using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetById;

internal sealed class GetEventVenueCalendarByIdQueryHandler(IEventVenueCalendarRepository repository)
        : IRequestHandler<GetEventVenueCalendarByIdQuery, EventVenueCalendarDto>
{
    public async Task<EventVenueCalendarDto> Handle(GetEventVenueCalendarByIdQuery request, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("EventVenueCalendar", request.Id);


        return EventVenueCalendarMapper.ToDto(entity);
    }
}