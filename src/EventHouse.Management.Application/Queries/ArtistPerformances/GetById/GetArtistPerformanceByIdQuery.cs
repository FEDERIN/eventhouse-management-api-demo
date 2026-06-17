using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.ArtistPerformances.GetById;

public sealed record GetArtistPerformanceByIdQuery(Guid Id)
    : IRequest<ArtistPerformanceDto>;
