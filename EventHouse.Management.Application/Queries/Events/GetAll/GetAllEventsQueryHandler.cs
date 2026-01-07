using MediatR;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers;

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
        } ;

        var result = await _eventRepository.GetPagedAsync(
            criteria,
            cancellationToken
        );

        var dtoList = result.Items.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Scope = EventScopeMapper.ToApplicationRequired(e.Scope)
        }).ToList();

        return new PagedResultDto<EventDto>
        {
            Items = dtoList,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }
}
