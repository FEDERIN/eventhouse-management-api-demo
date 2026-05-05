using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.SeatingMaps;
using MediatR;

namespace EventHouse.Management.Application.Queries.SeatingMaps.GetById;

internal class GetSeatingMapByIdQueryHandler(ISeatingMapRepository seatingMapRepository) 
    : IRequestHandler<GetSeatingMapByIdQuery, SeatingMapDto>
{
    public async Task<SeatingMapDto> Handle(GetSeatingMapByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await seatingMapRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("SeatingMap", request.Id);

        return SeatingMapMapper.ToDto(entity);
    }
}
