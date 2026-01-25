using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers;
using MediatR;
using static EventHouse.Management.Domain.Entities.Artist;

namespace EventHouse.Management.Application.Commands.Artists.AddGenre;

internal sealed class AddArtistGenreCommandHandler(
    IArtistRepository artistRepository,
    IGenreRepository genreRepository)
    : IRequestHandler<AddArtistGenreCommand>
{
    private readonly IArtistRepository _artistRepository = artistRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;

    public async Task Handle(AddArtistGenreCommand request, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetTrackedByIdAsync(request.ArtistId, cancellationToken)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        if (artist.Genres.Any(g => g.GenreId == request.GenreId))
            return;

        _ = await _genreRepository.GetByIdAsync(request.GenreId, cancellationToken)
                        ?? throw new NotFoundException("Genre", request.GenreId);

        var domainResult = artist.AddGenre(request.GenreId,
            ArtistGenreStatusMapper.ToDomainRequired(request.Status), request.IsPrimary);


        if (domainResult is AddGenreOutcome.Added)
            await _artistRepository.UpdateAsync(artist, cancellationToken);
    }
}
