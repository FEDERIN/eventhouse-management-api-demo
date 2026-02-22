using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Mappers.Venues;
using EventHouse.Management.Api.Mappers.Venues;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class VenueSortMapperTests
{
    [Theory]
    [InlineData(VenueSortBy.Name, VenueSortField.Name)]
    [InlineData(VenueSortBy.Address, VenueSortField.Address)]
    [InlineData(VenueSortBy.City, VenueSortField.City)]
    [InlineData(VenueSortBy.Region, VenueSortField.Region)]
    [InlineData(VenueSortBy.CountryCode, VenueSortField.CountryCode)]
    [InlineData(VenueSortBy.Capacity, VenueSortField.Capacity)]
    [InlineData(VenueSortBy.IsActive, VenueSortField.IsActive)]
    public void ToApplication_WhenSortByIsValid_ReturnsMappedField(
        VenueSortBy input,
        VenueSortField expected)
    {
        // Act
        var result = VenueSortMapper.ToApplication(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToApplication_WhenSortByIsNull_ReturnsNull()
    {
        // Act
        var result = VenueSortMapper.ToApplication(null);

        // Assert
        Assert.Null(result);
    }
}
