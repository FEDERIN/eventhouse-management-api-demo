using EventHouse.Management.Api.Contracts.Artists;
using EventHouse.Management.Application.Commands.Artists.Create;

namespace EventHouse.Management.Api.Mappers.Artists;

internal static class CreateArtistCommandMapper
{
    public static CreateArtistCommand FromContract(CreateArtistRequest request)
            => new(
            request.Name,
            ArtistCategoryMapper.ToApplicationRequired(request.Category));


}
