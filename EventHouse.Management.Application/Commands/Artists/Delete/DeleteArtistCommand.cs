using EventHouse.Management.Application.Common;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Delete
{
    public record DeleteArtistCommand(Guid Id) : IRequest<DeleteResult>;
}
