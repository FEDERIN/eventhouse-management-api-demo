using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.EventVenueCalendars.GetById
{
    public sealed record GetEventVenueCalendarByIdQuery(Guid Id) : IRequest<EventVenueCalendarDto>;
}
