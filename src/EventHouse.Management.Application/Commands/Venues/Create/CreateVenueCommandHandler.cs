using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Venues;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Create;

internal sealed class CreateVenueCommandHandler(IVenueRepository venueRepository)
    : IRequestHandler<CreateVenueCommand, VenueDto>
{
    public async Task<VenueDto> Handle(CreateVenueCommand request, CancellationToken ct)
    {
        var entity = VenueMapper.ToEntity(request);

        await venueRepository.AddAsync(entity, ct);

        return VenueMapper.ToDto(entity);
    }
}