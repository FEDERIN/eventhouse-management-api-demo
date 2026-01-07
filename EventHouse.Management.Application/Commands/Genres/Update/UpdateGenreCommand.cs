using EventHouse.Management.Application.Common;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Update
{
    public record UpdateGenreCommand(
        Guid Id,
        string Name
        ) : IRequest<UpdateResult>;
}
