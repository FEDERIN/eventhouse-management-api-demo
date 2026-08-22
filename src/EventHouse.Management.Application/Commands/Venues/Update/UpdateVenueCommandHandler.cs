using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Update;

internal sealed class UpdateVenueCommandHandler(
    IVenueRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<UpdateVenueCommand>
{
    public async Task Handle(
        UpdateVenueCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var entity = await repository.GetTrackedByIdAsync(
                    request.Id,
                    ct)
                    ?? throw new NotFoundException(
                        "Venue",
                        request.Id);

                entity.Update(
                    request.Name,
                    request.Address,
                    request.City,
                    request.Region,
                    request.CountryCode,
                    request.Latitude,
                    request.Longitude,
                    request.TimeZoneId,
                    request.Capacity,
                    request.IsActive);

                await repository.UpdateAsync(entity, ct);
            },
            ct);
    }
}