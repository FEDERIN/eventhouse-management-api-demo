using ApiSortDirection = EventHouse.Management.Api.Contracts.Common.SortDirection;
using AppSortDirection = EventHouse.Management.Application.Common.Sorting.SortDirection;

namespace EventHouse.Management.Api.Mappers;

public static class SortDirectionMapper
{
    public static AppSortDirection ToApplication(ApiSortDirection direction) =>
        ApiEnumMapper<ApiSortDirection, AppSortDirection>.ToApplicationRequired(direction);
}