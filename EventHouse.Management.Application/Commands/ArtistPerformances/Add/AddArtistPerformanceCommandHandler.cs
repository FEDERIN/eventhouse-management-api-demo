using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.ArtistPerformances;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Add;

internal sealed class AddArtistPerformanceCommandHandler(
    IArtistPerformanceRepository artistPerformanceRepository,
    IEventVenueCalendarRepository eventVenueCalendarRepository,
    IArtistRepository artistRepository)
    : IRequestHandler<AddArtistPerformanceCommand, ArtistPerformanceDto>
{
    public async Task<ArtistPerformanceDto> Handle(
        AddArtistPerformanceCommand request,
        CancellationToken ct)
    {
        var eventVenueCalendar = await eventVenueCalendarRepository.GetByIdWithPerformancesAsync(request.EventVenueCalendarId, ct)
            ?? throw new NotFoundException(nameof(EventVenueCalendar), request.EventVenueCalendarId);

        var artistExists = await artistRepository.ExistsAsync(request.ArtistId, ct);

        if (!artistExists)
            throw new NotFoundException("Artist", request.ArtistId);

        var domainResult = eventVenueCalendar.AddPerformance(
                request.ArtistId,
                request.IsHeadliner,
                request.SetStart,
                request.SetEnd);

        if(domainResult == AddCalendarOutcome.NoChange)
            throw new ConflictException("ARTIST_ALREADY_ADDED", "Artist Conflict", "The artist is already added to this event.");


        if (eventVenueCalendar.Status == EventVenueCalendarStatus.Published &&
            request.SetStart.HasValue && request.SetEnd.HasValue)
        {
            var isBusy = await artistPerformanceRepository.IsArtistBusyAsync(
                request.ArtistId,
                null,
                request.SetStart.Value.UtcDateTime,
                request.SetEnd.Value.UtcDateTime,
                ct);

            if (isBusy)
                throw new ConflictException("ARTIST_UNAVAILABLE", "Artist Conflict", "The artist is already booked in another event.");
        }
        
        await eventVenueCalendarRepository.UpdateAsync(eventVenueCalendar, ct);

  
        var newPerformance = eventVenueCalendar.Performances.First(p => p.ArtistId == request.ArtistId);

        return ArtistPerformanceMapper.ToDto(newPerformance);
    }
}