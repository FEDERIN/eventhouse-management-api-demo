using EventHouse.Management.Api.Contracts.Genres;
using EventHouse.Management.Application.Commands.Genres.Create;

namespace EventHouse.Management.Api.Mappers.Genres;

internal static class CreateGenreCommandMapper
{
    public static CreateGenreCommand FromContract(CreateGenreRequest request)
        => new(request.Name);
}
