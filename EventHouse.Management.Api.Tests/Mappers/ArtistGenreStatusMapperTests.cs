using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Api.Mappers.Artists;
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Api.Tests.Mappers;

public sealed class ArtistGenreStatusMapperTests
    : ApiEnumMapperTestBase<ArtistGenreStatus, ArtistGenreStatusDto>
{
    protected override ArtistGenreStatusDto ToApplicationRequired(ArtistGenreStatus contract) =>
        ArtistGenreStatusMapper.ToApplicationRequired(contract);

    protected override ArtistGenreStatusDto? ToApplicationOptional(ArtistGenreStatus? contract) =>
        ArtistGenreStatusMapper.ToApplicationOptional(contract);

    protected override ArtistGenreStatus ToContractRequired(ArtistGenreStatusDto dto) =>
        ArtistGenreStatusMapper.ToContract(dto);
}