using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Events.GetById;

public sealed record GetEventByIdQuery(Guid Id) : IRequest<EventDto?>;
