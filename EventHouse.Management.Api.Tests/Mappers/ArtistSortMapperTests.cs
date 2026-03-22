using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Application.Queries.Artists.GetAll;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class ArtistSortMapperTests
    : ApiEnumMapperUnidirectionalTestBase<ArtistSortBy, ArtistSortField>
{
    protected override ArtistSortField ToApplicationRequired(ArtistSortBy contract) =>
        ArtistSortMapper.ToApplication(contract)
        ?? throw new ArgumentNullException(nameof(contract), "Mapping failed unexpectedly.");

    protected override ArtistSortField? ToApplicationOptional(ArtistSortBy? contract) =>
        ArtistSortMapper.ToApplication(contract);
}