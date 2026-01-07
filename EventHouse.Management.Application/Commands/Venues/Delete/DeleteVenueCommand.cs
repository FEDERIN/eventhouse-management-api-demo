using EventHouse.Management.Application.Common;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete
{
    public record DeleteVenueCommand(Guid Id) : IRequest<DeleteResult>;
}
