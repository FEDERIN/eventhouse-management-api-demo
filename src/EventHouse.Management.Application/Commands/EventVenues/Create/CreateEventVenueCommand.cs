using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenues.Create;

public sealed record CreateEventVenueCommand(
    Guid EventId,
    Guid VenueId,
    EventVenueStatusDto Status
) : IRequest<EventVenueDto>;
