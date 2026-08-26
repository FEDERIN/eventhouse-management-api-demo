using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs.Seating.Structure;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.SeatingMaps;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetStructure;

public sealed class GetSeatingMapStructureQueryHandler(
    ISeatingMapRepository repository)
        : IRequestHandler<GetSeatingMapStructureQuery, SeatingMapStructureDto>
{

    public async Task<SeatingMapStructureDto> Handle(
        GetSeatingMapStructureQuery request,
        CancellationToken ct)
    {
        var seatingMap = await repository
            .GetWithStructureByIdAsync(request.SeatingMapId, ct);

        return seatingMap is null
            ? throw new NotFoundException(
                        nameof(SeatingMap),
                        request.SeatingMapId)
            : SeatingMapMapper.ToStructureDto(seatingMap);
    }
}