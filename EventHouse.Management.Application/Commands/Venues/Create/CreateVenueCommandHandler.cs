using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Create;

internal sealed class CreateVenueCommandHandler(IVenueRepository venueRepository)
    : IRequestHandler<CreateVenueCommand, VenueDto>
{
    private readonly IVenueRepository _venueRepository = venueRepository;

    public async Task<VenueDto> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
    {
        var entity = new Venue(
            id: Guid.NewGuid(),
            name: request.Name,
            address: request.Address,
            city: request.City,
            region: request.Region,
            countryCode: request.CountryCode,
            latitude: request.Latitude,
            longitude: request.Longitude,
            timeZoneId: request.TimeZoneId,
            capacity: request.Capacity,
            isActive: request.IsActive
        );

        await _venueRepository.AddAsync(entity, cancellationToken);

        return new VenueDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            City = entity.City,
            Region = entity.Region,
            CountryCode = entity.CountryCode,
            Capacity = entity.Capacity,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            TimeZoneId = entity.TimeZoneId,
            IsActive = entity.IsActive
        };
    }
}
