using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenues;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenues.GetById;

internal sealed class GetEventVenueByIdQueryHandler(IEventVenueRepository repository)
            : IRequestHandler<GetEventVenueByIdQuery, EventVenueDto>
{
    public async Task<EventVenueDto> Handle(GetEventVenueByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("EventVenue", request.Id);

        return EventVenueMapper.ToDto(entity);
    }
}
