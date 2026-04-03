
using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;

public sealed record UpdateEventVenueStatusCommand(
    Guid Id,
    EventVenueStatusDto Status
) : IRequest;
