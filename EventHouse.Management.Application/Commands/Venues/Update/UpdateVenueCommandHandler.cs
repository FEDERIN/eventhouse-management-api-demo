using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandHandler(IVenueRepository VenueRepository) : IRequestHandler<UpdateVenueCommand>
{
    private readonly IVenueRepository _VenueRepository = VenueRepository;

    public async Task Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _VenueRepository.GetTrackedByIdAsync(request.Id, cancellationToken)
        ?? throw new NotFoundException("Venue", request.Id);

        entity.Update(request.Name, request.Address, request.City, request.Region, request.CountryCode,
        request.Latitude, request.Longitude, request.TimeZoneId, request.Capacity, request.IsActive);

        await _VenueRepository.UpdateAsync(entity, cancellationToken);
    }
}
