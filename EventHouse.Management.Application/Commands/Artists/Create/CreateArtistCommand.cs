using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.DTOs;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Create;

public sealed record CreateArtistCommand(
    string Name,
    ArtistCategory Category
) : IRequest<ArtistDto>;
