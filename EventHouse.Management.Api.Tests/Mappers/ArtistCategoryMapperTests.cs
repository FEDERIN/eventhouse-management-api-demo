using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Api.Mappers.Artists;

namespace EventHouse.Management.Api.Tests.Mappers;


public sealed class ArtistCategoryMapperTests
    : ApiEnumMapperTestBase<ArtistCategory, ArtistCategoryDto>
{
    protected override ArtistCategoryDto ToApplicationRequired(ArtistCategory contract) =>
        ArtistCategoryMapper.ToApplicationRequired(contract);

    protected override ArtistCategoryDto? ToApplicationOptional(ArtistCategory? contract) =>
        ArtistCategoryMapper.ToApplicationOptional(contract);

    protected override ArtistCategory ToContractRequired(ArtistCategoryDto dto) =>
        ArtistCategoryMapper.ToContract(dto);
}
