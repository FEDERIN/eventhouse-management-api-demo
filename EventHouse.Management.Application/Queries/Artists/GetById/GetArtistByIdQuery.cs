using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Queries.Artists.GetById
{
    public sealed record GetArtistByIdQuery(Guid Id) : IRequest<ArtistDto>;
}
