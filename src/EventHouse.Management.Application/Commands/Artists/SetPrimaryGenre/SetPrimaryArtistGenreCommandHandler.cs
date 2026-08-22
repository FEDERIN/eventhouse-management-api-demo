using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

internal sealed class SetPrimaryArtistGenreCommandHandler(
    IArtistRepository repository,
    IApplicationResilience resilience)
    : IRequestHandler<SetPrimaryArtistGenreCommand>
{
    public async Task Handle(
        SetPrimaryArtistGenreCommand request,
        CancellationToken ct)
    {
        await resilience.ExecuteSqlAsync(
            async ct =>
            {
                var artist = await repository.GetByIdAsync(
                    request.ArtistId,
                    ct)
                    ?? throw new NotFoundException(
                        "Artist",
                        request.ArtistId);

                var currentPrimaryGenreId =
                    artist.Genres
                        .FirstOrDefault(g => g.IsPrimary)?
                        .GenreId ?? Guid.Empty;

                var changed =
                    artist.CanSetPrimaryGenre(request.GenreId);

                if (!changed)
                    return;

                await repository.SetPrimaryGenreAsync(
                    artist.Id,
                    currentPrimaryGenreId,
                    request.GenreId,
                    ct);
            },
            ct);
    }
}