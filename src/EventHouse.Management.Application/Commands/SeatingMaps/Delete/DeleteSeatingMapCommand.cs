using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Delete;

public sealed record DeleteSeatingMapCommand(Guid Id) : IRequest;
