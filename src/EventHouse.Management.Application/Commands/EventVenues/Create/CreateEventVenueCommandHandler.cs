using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.EventVenues;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenues.Create;

internal sealed class CreateEventVenueCommandHandler
    (IEventVenueRepository eventVenueRepository, IEventRepository eventRepository, IVenueRepository venueRepository) 
    : IRequestHandler<CreateEventVenueCommand, EventVenueDto>
{
    public async Task<EventVenueDto> Handle(CreateEventVenueCommand request, CancellationToken ct)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId, ct)
            ?? throw new NotFoundException("Event", request.EventId);

        var venue = await venueRepository.GetByIdAsync(request.VenueId, ct)
            ?? throw new NotFoundException("Venue", request.VenueId);

        var entity = EventVenueMapper.ToEntity(request);

        await eventVenueRepository.AddAsync(entity, ct);

        return EventVenueMapper.ToDto(entity, @event.Name, venue.Name);
    }
}