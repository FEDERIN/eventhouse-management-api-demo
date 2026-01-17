using EventHouse.Management.Api.Mappers;

using ApiSortDirection = EventHouse.Management.Api.Contracts.Common.SortDirection;
using AppSortDirection = EventHouse.Management.Application.Common.Sorting.SortDirection;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class SortDirectionMapperTests
{
    [Fact]
    public void ToApplication_WhenDesc_ReturnsDesc()
    {
        var result = SortDirectionMapper.ToApplication(ApiSortDirection.Desc);

        Assert.Equal(AppSortDirection.Desc, result);
    }

    [Fact]
    public void ToApplication_WhenNotDesc_ReturnsAsc()
    {
        // Asc o cualquier otro valor cae en el default
        var result = SortDirectionMapper.ToApplication(ApiSortDirection.Asc);

        Assert.Equal(AppSortDirection.Asc, result);
    }
}
