

using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

public sealed record CreateSeatingMapCommand
(
    Guid VenueId,
    string Name,
    int Version
) : IRequest<SeatingMapDto>;
