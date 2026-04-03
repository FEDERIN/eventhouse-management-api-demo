using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenueCalendars;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetById;

internal sealed class GetEventVenueCalendarByIdQueryHandler(IEventVenueCalendarRepository repository)
        : IRequestHandler<GetEventVenueCalendarByIdQuery, EventVenueCalendarDto>
{
    private readonly IEventVenueCalendarRepository _repository = repository;

    public async Task<EventVenueCalendarDto> Handle(GetEventVenueCalendarByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("EventVenueCalendar", request.Id);


        return EventVenueCalendarMapper.ToDto(entity);
    }
}
