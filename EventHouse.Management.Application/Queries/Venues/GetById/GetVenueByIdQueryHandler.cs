using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetById;

internal sealed class GetVenueByIdQueryHandler(IVenueRepository repository)
            : IRequestHandler<GetVenueByIdQuery, VenueDto>
{
    private readonly IVenueRepository _repository = repository;

    public async Task<VenueDto> Handle(GetVenueByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Venue", request.Id);

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
