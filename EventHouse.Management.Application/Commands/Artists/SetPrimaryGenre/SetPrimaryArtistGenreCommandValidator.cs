
namespace EventHouse.Management.Application.Commands.Artists.SetPrimaryGenre;

internal sealed class SetPrimaryArtistGenreCommandValidator
    : ArtistGenreCommandValidatorBase<SetPrimaryArtistGenreCommand>
{
    public SetPrimaryArtistGenreCommandValidator()
    {
        ApplyArtistGenreRules(
            cmd => cmd.ArtistId,
            cmd => cmd.GenreId);
    }
}
