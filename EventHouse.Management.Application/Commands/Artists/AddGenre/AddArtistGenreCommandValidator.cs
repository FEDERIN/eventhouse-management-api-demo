using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists.AddGenre;

internal sealed class AddArtistGenreCommandValidator
    : ArtistGenreCommandValidatorBase<AddArtistGenreCommand>
{
    public AddArtistGenreCommandValidator()
    {
        ApplyArtistGenreRules(
            cmd => cmd.ArtistId,
            cmd => cmd.GenreId);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid ArtistGenreStatus value.");
    }
}

