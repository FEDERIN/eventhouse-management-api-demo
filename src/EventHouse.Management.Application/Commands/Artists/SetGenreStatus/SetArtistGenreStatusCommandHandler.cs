using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.Artists;
using MediatR;

namespace EventHouse.Management.Application.Commands.Artists.SetGenreStatus;

internal sealed class SetArtistGenreStatusCommandHandler(IArtistRepository artistRepository)
    : IRequestHandler<SetArtistGenreStatusCommand>
{
    public async Task Handle(SetArtistGenreStatusCommand request, CancellationToken ct)
    {
        var artist = await artistRepository.GetTrackedByIdAsync(request.ArtistId, ct)
            ?? throw new NotFoundException("Artist", request.ArtistId);

        var changed = artist.SetGenreStatus(request.GenreId, ArtistGenreStatusMapper.ToDomainRequired(request.Status));

        if(changed)
            await artistRepository.UpdateAsync(artist, ct);
    }
}
