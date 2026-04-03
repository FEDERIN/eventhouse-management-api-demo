

using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

public record CreateSeatingMapCommand
(
    Guid VenueId,
    string Name,
    int Version,
    bool IsActive
) : IRequest<SeatingMapDto>;
