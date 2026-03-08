using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenues;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenues.Create;

internal sealed class CreateEventVenueCommandHandler
    (IEventVenueRepository eventVenueRepository, IEventRepository eventRepository, IVenueRepository venueRepository) 
    : IRequestHandler<CreateEventVenueCommand, EventVenueDto>
{
    private readonly IEventVenueRepository _eventVenueRepository = eventVenueRepository;

    public async Task<EventVenueDto> Handle(CreateEventVenueCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException("Event", request.EventId);

        var venue = await venueRepository.GetByIdAsync(request.VenueId, cancellationToken)
            ?? throw new NotFoundException("Venue", request.VenueId);

        var entity = new EventVenue(
            id: Guid.NewGuid(),
            eventId: request.EventId,
            venueId: request.VenueId,
            status: EventVenueStatusMapper.ToDomainRequired(request.Status)
        );

        await _eventVenueRepository.AddAsync(entity, cancellationToken);

        return EventVenueMapper.ToDto(entity, @event.Name, venue.Name);
    }
}
