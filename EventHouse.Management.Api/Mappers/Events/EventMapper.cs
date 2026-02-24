using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Events;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.Events;

public static class EventMapper
{
    public static EventResponse ToContract(EventDto dto)
    {
        return new EventResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Scope = EventScopeMapper.ToContractRequired(dto.Scope),
        };
    }

    public static PagedResult<EventResponse> ToContract(
    PagedResultDto<EventDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
