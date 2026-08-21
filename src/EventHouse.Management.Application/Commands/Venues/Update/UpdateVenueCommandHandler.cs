using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandHandler(IVenueRepository venueRepository) : IRequestHandler<UpdateVenueCommand>
{
    public async Task Handle(UpdateVenueCommand request, CancellationToken ct)
    {
        var entity = await venueRepository.GetTrackedByIdAsync(request.Id, ct)
        ?? throw new NotFoundException("Venue", request.Id);

        entity.Update(request.Name, request.Address, request.City, request.Region, request.CountryCode,
        request.Latitude, request.Longitude, request.TimeZoneId, request.Capacity, request.IsActive);

        await venueRepository.UpdateAsync(entity, ct);
    }
}
