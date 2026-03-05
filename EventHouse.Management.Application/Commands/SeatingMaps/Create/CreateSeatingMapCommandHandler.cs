
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.SeatingMaps;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

internal sealed class CreateSeatingMapCommandHandler(ISeatingMapRepository seatingMapRepository, IVenueRepository venueRepository)
    : IRequestHandler<CreateSeatingMapCommand, SeatingMapDto>
{
    public async Task<SeatingMapDto> Handle(CreateSeatingMapCommand request, CancellationToken cancellationToken)
    {
        var venueExists = await venueRepository.ExistsAsync(request.VenueId, cancellationToken);
        if (!venueExists)
            throw new NotFoundException("Venue", request.VenueId);

        var entity = new SeatingMap(
             Guid.NewGuid(),
            request.VenueId,
            request.Name,
            request.Version == 0 ? 1 : request.Version,
            request.IsActive
        );
        
        await seatingMapRepository.AddAsync(entity, cancellationToken);

        return SeatingMapMapper.ToDto(entity);
    }
}
