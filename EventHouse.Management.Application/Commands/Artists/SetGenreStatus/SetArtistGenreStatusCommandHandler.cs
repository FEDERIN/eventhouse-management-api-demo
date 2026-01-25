using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetGenreStatus;

internal sealed class SetArtistGenreStatusCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<SetArtistGenreStatusCommand>
{
    private readonly IArtistRepository _artistRepository = artistRepository;

    public async Task Handle(SetArtistGenreStatusCommand request, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetTrackedByIdAsync(request.ArtistId, cancellationToken)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        var changed = artist.SetGenreStatus(request.GenreId, ArtistGenreStatusMapper.ToDomainRequired(request.Status));

        if(changed)
            await _artistRepository.UpdateAsync(artist, cancellationToken);
    }
}
