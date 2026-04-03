using EventHouse.Management.Api.Mappers;

using ApiSortDirection = EventHouse.Management.Api.Contracts.Common.SortDirection;
using AppSortDirection = EventHouse.Management.Application.Common.Sorting.SortDirection;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class SortDirectionMapperTests
    : ApiEnumMapperUnidirectionalTestBase<ApiSortDirection, AppSortDirection>
{
    protected override AppSortDirection ToApplicationRequired(ApiSortDirection contract) =>
        SortDirectionMapper.ToApplication(contract);

    protected override AppSortDirection? ToApplicationOptional(ApiSortDirection? contract) =>
        contract.HasValue ? SortDirectionMapper.ToApplication(contract.Value) : null;


}
