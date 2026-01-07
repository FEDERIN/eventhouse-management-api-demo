using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Venues.GetById
{
    public sealed record GetVenueByIdQuery(Guid Id) : IRequest<VenueDto>;
}
