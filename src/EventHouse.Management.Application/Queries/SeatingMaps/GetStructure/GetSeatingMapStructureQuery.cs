using EventHouse.Management.Application.DTOs.Seating.Structure;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetStructure;

public sealed record GetSeatingMapStructureQuery(
    Guid SeatingMapId) : IRequest<SeatingMapStructureDto>;