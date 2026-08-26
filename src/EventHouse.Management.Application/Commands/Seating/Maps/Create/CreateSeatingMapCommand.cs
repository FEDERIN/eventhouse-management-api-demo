using EventHouse.Management.Application.DTOs.Seating;
using MediatR;

namespace EventHouse.Management.Application.Commands.Seating.Maps.Create;

public sealed record CreateSeatingMapCommand
(
    Guid VenueId,
    string Name,
    int Version
) : IRequest<SeatingMapDto>;
