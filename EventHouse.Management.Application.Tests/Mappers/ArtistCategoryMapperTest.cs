using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Mappers.Artists;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Application.Tests.Mappers;

public sealed class ArtistCategoryMapperTests
    : EnumMapperTestBase<ArtistCategory, ArtistCategoryDto>
{
    protected override ArtistCategory ToDomainRequired(ArtistCategoryDto dto) =>
        ArtistCategoryMapper.ToDomainRequired(dto);

    protected override ArtistCategory? ToDomainOptional(ArtistCategoryDto? dto) =>
        ArtistCategoryMapper.ToDomainOptional(dto);

    protected override ArtistCategoryDto ToApplicationRequired(ArtistCategory domain) =>
        ArtistCategoryMapper.ToApplication(domain);
}