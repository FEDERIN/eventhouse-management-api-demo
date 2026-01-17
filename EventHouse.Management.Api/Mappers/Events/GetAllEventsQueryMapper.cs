using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Queries.Events.GetAll;

namespace EventHouse.Management.Api.Mappers.Events;

public static class GetAllEventsQueryMapper
{
    public static GetAllEventsQuery FromContract(GetEventsRequest request)
        => new()
        {
            Name = request.Name,
            Description = request.Description,
            Scope = EventScopeMapper.ToApplicationOptional(request.Scope),
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = EventSortMapper.ToApplication(request.SortBy),
            SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
        };
}
