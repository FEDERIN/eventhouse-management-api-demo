using EventHouse.Management.Application.Common.Enums;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.Update;

public sealed record UpdateArtistCommand
    (
    Guid Id,
    string Name,
    ArtistCategoryDto Category
    ) : IRequest;
