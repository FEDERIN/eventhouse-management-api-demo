using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Mappers.Artists;
using EventHouse.Management.Domain.Exceptions;
using MediatR;
using static EventHouse.Management.Domain.Entities.Artist;

namespace EventHouse.Management.Application.Commands.Artists.AddGenre;

internal sealed class AddArtistGenreCommandHandler(
    IArtistRepository artistRepository,
    IGenreRepository genreRepository)
    : IRequestHandler<AddArtistGenreCommand>
{
    public async Task Handle(AddArtistGenreCommand request, CancellationToken ct)
    {
        var artist = await artistRepository.GetTrackedByIdAsync(request.ArtistId, ct)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        if (artist.Genres.Any(g => g.GenreId == request.GenreId))
            return;

        _ = await genreRepository.GetByIdAsync(request.GenreId, ct)
                        ?? throw new NotFoundException("Genre", request.GenreId);

        var domainResult = artist.AddGenre(request.GenreId,
            ArtistGenreStatusMapper.ToDomainRequired(request.Status), request.IsPrimary);


        if (domainResult is AddGenreOutcome.Added)
            await artistRepository.UpdateAsync(artist, ct);
    }
}