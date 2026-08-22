using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.SeatingMaps;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Create;

internal sealed class CreateSeatingMapCommandHandler(
    ISeatingMapRepository seatingMapRepository,
    IVenueRepository venueRepository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateSeatingMapCommand, SeatingMapDto>
{
    public Task<SeatingMapDto> Handle(
        CreateSeatingMapCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var venueExists = await venueRepository.ExistsAsync(
                    request.VenueId,
                    ct);

                if (!venueExists)
                    throw new NotFoundException("Venue", request.VenueId);

                var entity = SeatingMapMapper.ToEntity(request);

                await seatingMapRepository.AddAsync(entity, ct);

                return SeatingMapMapper.ToDto(entity);
            },
            ct);
    }
}