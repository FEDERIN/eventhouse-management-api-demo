using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Api.Mappers.Common;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.DTOs;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal sealed class SeatingMapMapper
{
    public static SeatingMapResponse ToContract(SeatingMapDto seatingMap)
    {
        return new SeatingMapResponse
        {
            Id = seatingMap.Id,
            VenueId = seatingMap.VenueId,
            Name = seatingMap.Name,
            Version = seatingMap.Version,
            IsActive = seatingMap.IsActive,
            CreatedAtUtc = seatingMap.CreatedAtUtc
        };
    }

    public static PagedResult<SeatingMapResponse> ToContract(
    PagedResultDto<SeatingMapDto> paged, HttpRequest request)
    => paged.ToContract(ToContract, request);
}
