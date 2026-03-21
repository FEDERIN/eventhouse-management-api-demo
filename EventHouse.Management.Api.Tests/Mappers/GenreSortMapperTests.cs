using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Api.Mappers.Genres;
using EventHouse.Management.Application.Queries.Genres.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class GenreSortMapperTests
    : ApiEnumMapperTestBase<GenreSortBy, GenreSortField>
{
    protected override GenreSortField ToApplicationRequired(GenreSortBy contract) =>
        GenreSortMapper.ToApplication(contract)
        ?? throw new ArgumentNullException(nameof(contract), "Mapping failed unexpectedly.");

    protected override GenreSortField? ToApplicationOptional(GenreSortBy? contract) =>
        GenreSortMapper.ToApplication(contract);

    protected override GenreSortBy ToContractRequired(GenreSortField dto)
    {
        throw new NotImplementedException("Sort mapping is unidirectional (Contract to App).");
    }
}