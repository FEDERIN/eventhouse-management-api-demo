
namespace EventHouse.Management.Application.Commands.Artists.RemoveGenre;

internal sealed class RemoveArtistGenreCommandValidator
    : ArtistGenreCommandValidatorBase<RemoveArtistGenreCommand>
{
    public RemoveArtistGenreCommandValidator()
    {
        ApplyArtistGenreRules(
            cmd => cmd.ArtistId,
            cmd => cmd.GenreId);
    }
}
