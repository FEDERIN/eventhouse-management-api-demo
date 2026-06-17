using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenues.GetById
{
    public sealed record GetEventVenueByIdQuery(Guid Id) : IRequest<EventVenueDto>;
}
