using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Venues;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Create;

internal sealed class CreateVenueCommandHandler(
    IVenueRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<CreateVenueCommand, VenueDto>
{
    public Task<VenueDto> Handle(
        CreateVenueCommand request,
        CancellationToken ct)
    {
        return resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = VenueMapper.ToEntity(request);

                await repository.AddAsync(entity, ct);

                return VenueMapper.ToDto(entity);
            },
            ct);
    }
}