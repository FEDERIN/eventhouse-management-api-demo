using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetById;

internal sealed class GetVenueByIdQueryHandler(IVenueRepository repository)
            : IRequestHandler<GetVenueByIdQuery, VenueDto>
{
    private readonly IVenueRepository _repository = repository;

    public async Task<VenueDto> Handle(GetVenueByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Venue with Id '{request.Id}' was not found.");

        return new VenueDto() {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            City = entity.City,
            Region = entity.Region,
            CountryCode = entity.CountryCode,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            Capacity = entity.Capacity,
            TimeZoneId = entity.TimeZoneId,
            IsActive = entity.IsActive
        };
    }
}
