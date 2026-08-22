using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.ArtistPerformances;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;

internal sealed class GetAllArtistPerformancesQueryHandler(IArtistPerformanceRepository artistPerformanceRepository, IEventVenueCalendarRepository eventVenueCalendarRepository)
    : IRequestHandler<GetAllArtistPerformancesQuery, PagedResultDto<ArtistPerformanceDto>>
{
    public async Task<PagedResultDto<ArtistPerformanceDto>> Handle(GetAllArtistPerformancesQuery request, CancellationToken ct)
    {
        var calendarExists = await eventVenueCalendarRepository.ExistsAsync(request.EventVenueCalendarId, ct);

        if (!calendarExists)
        {
            throw new NotFoundException(nameof(EventVenueCalendar), request.EventVenueCalendarId);
        }

        var criteria = new ArtistPerformanceQueryCriteria
        {
            EventVenueCalendarId = request.EventVenueCalendarId,
            ArtistId = request.ArtistId,
            IsHeadliner = request.IsHeadliner,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await artistPerformanceRepository.GetPagedAsync(criteria, ct);

        return result.MapTo(ArtistPerformanceMapper.ToDto);
    }
}
