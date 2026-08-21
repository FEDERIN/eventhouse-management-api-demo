using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Mappers.Venues;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetById;

internal sealed class GetVenueByIdQueryHandler(IVenueRepository repository)
            : IRequestHandler<GetVenueByIdQuery, VenueDto>
{
    public async Task<VenueDto> Handle(GetVenueByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Venue", request.Id);

        return VenueMapper.ToDto(entity);
    }
}