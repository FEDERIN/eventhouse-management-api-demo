using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Delete;

public sealed record DeleteArtistCommand(Guid Id) : IRequest;
