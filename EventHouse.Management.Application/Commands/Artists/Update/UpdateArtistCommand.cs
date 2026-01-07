using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update
{
    public record UpdateArtistCommand
        (
        Guid Id,
        string Name,
        ArtistCategory Category
        ) : IRequest<UpdateResult>;
}
