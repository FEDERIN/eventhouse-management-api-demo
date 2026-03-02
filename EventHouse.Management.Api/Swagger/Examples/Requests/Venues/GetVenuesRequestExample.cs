using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Api.Contracts.Venues;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.Venues;

[ExcludeFromCodeCoverage]
public class GetVenuesRequestExample : IExamplesProvider<GetVenuesRequest>
{
    public GetVenuesRequest GetExamples() => new()
    {
        Name = "Madison",
        Address = "123 Main St",
        City = "New York",
        Region = "NY",
        CountryCode = "US",
        Capacity = 20000,
        IsActive = true,
        Page = 1,
        PageSize = 15,
        SortBy = VenueSortBy.Name,
        SortDirection = SortDirection.Asc
    };
}
