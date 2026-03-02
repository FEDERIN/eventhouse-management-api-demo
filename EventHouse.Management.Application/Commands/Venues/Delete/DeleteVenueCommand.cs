using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete
{
    public record DeleteVenueCommand(Guid Id) : IRequest;
}
