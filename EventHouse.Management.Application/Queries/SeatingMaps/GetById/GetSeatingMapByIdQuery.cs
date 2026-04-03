using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetById;

public sealed record GetSeatingMapByIdQuery(Guid Id) : IRequest<SeatingMapDto>;

