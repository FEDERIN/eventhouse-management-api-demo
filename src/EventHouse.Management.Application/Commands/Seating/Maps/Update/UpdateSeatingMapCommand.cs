using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Maps.Update;

public sealed record class UpdateSeatingMapCommand
(
    Guid Id,
    string Name,
    int Version
) : IRequest;

