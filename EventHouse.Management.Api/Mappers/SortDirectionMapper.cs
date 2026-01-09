using ApiSortDirection = EventHouse.Management.Api.Contracts.Common.SortDirection;
using AppSortDirection = EventHouse.Management.Application.Common.Sorting.SortDirection;

namespace EventHouse.Management.Api.Mappers;

public static class SortDirectionMapper
{
    public static AppSortDirection ToApplication(ApiSortDirection direction)
        => direction switch
        {
            ApiSortDirection.Desc => AppSortDirection.Desc,
            _ => AppSortDirection.Asc
        };
}
