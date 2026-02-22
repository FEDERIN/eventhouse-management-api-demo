using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Venues;
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

        return VenuesMapper.ToDto(entity);
    }
}
