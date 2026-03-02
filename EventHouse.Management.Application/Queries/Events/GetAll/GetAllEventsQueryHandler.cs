using MediatR;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers.Events;

namespace EventHouse.Management.Application.Queries.Events.GetAll;

internal sealed class GetAllEventsQueryHandler(IEventRepository eventRepository)
    : IRequestHandler<GetAllEventsQuery, PagedResultDto<EventDto>>
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<PagedResultDto<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var criteria = new EventQueryCriteria
        {
            Name = request.Name,
            Description = request.Description,
            Scope = EventScopeMapper.ToDomainOptional(request.Scope),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var result = await _eventRepository.GetPagedAsync(
            criteria,
            cancellationToken
        );

        return new PagedResultDto<EventDto>
        {
            Items = EventsMapper.ToDto(result.Items),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
