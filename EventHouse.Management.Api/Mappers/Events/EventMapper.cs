using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Api.Mappers.Enums;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;
using Contract = EventHouse.Management.Api.Contracts.Events.Event;

namespace EventHouse.Management.Api.Mappers.Events;

public static class EventMapper
{
    public static Contract ToContract(EventDto dto)
    {
        return new Contract
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Scope = EventScopeMapper.ToContractRequired(dto.Scope),
        };
    }

    public static PagedResult<Contract> ToContract(
    PagedResultDto<EventDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
