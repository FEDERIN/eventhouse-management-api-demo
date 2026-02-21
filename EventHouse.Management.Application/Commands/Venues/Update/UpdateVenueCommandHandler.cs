using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandHandler(IVenueRepository VenueRepository) : IRequestHandler<UpdateVenueCommand, UpdateResult>
{
    private readonly IVenueRepository _VenueRepository = VenueRepository;

    public async Task<UpdateResult> Handle(UpdateVenueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _VenueRepository.GetTrackedByIdAsync(request.Id, cancellationToken)
        ?? throw new KeyNotFoundException($"Venue with Id '{request.Id}' was not found.");

        entity.Update(request.Name, request.Address, request.City, request.Region, request.CountryCode,
        request.Latitude, request.Longitude, request.TimeZoneId, request.Capacity, request.IsActive);

        await _VenueRepository.UpdateAsync(entity, cancellationToken);

        return UpdateResult.Success;
    }
}
