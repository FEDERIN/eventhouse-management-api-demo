using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.EventVenues;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenues.GetAll;

internal sealed class GetAllEventVenuesQueryHandler
    (IEventVenueRepository repository, IEventRepository eventRepository,IVenueRepository venueRepository)
            : IRequestHandler<GetAllEventVenuesQuery, PagedResultDto<EventVenueDto>>
{
    public async Task<PagedResultDto<EventVenueDto>> Handle(
        GetAllEventVenuesQuery request,
        CancellationToken ct)
    {
        string? cachedEventName = null;
        string? cachedVenueName = null;

        if (request.EventId.HasValue)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId.Value, ct) ?? throw new NotFoundException("Event", request.EventId.Value);
            cachedEventName = @event?.Name;
        }

        if (request.VenueId.HasValue)
        {
            var venue = await venueRepository.GetByIdAsync(request.VenueId.Value, ct) 
                ?? throw new NotFoundException("Venue", request.VenueId.Value);

            cachedVenueName = venue?.Name;
        }

        var criteria = new EventVenueQueryCriteria
        {
            EventId = request.EventId,
            VenueId = request.VenueId,
            Status = EventVenueStatusMapper.ToDomainOptional(request.Status),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await repository.GetPagedAsync(criteria, ct);

        return new PagedResultDto<EventVenueDto>
        {
            Items = EventVenueMapper.ToDtoList(result.Items, cachedEventName, cachedVenueName),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

}
