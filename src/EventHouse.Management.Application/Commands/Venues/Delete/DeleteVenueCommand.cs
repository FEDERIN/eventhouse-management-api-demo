using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete;

public sealed record DeleteVenueCommand(Guid Id) : IRequest;
