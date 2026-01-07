using EventHouse.Management.Application.Common;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete
{
    public record DeleteGenreCommand(Guid Id) : IRequest<DeleteResult>;
}
