using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Api.Mappers.Venues;
using EventHouse.Management.Application.Queries.Venues.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class VenueSortMapperTests
    : ApiEnumMapperTestBase<VenueSortBy, VenueSortField>
{
    protected override VenueSortField ToApplicationRequired(VenueSortBy contract) =>
        VenueSortMapper.ToApplication(contract)
        ?? throw new ArgumentNullException(nameof(contract), "Mapping failed unexpectedly.");

    protected override VenueSortField? ToApplicationOptional(VenueSortBy? contract) =>
        VenueSortMapper.ToApplication(contract);

    protected override VenueSortBy ToContractRequired(VenueSortField dto)
    {
        throw new NotImplementedException("Sort mapping is unidirectional (Contract to App).");
    }
}