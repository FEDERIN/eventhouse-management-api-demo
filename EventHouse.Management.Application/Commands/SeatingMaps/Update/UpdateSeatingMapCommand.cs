
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Update;

public sealed record class UpdateSeatingMapCommand
(
    Guid Id,
    string Name,
    int Version,
    bool IsActive
) : IRequest;

