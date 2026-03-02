using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists.SetGenreStatus;

internal sealed class SetArtistGenreStatusCommandValidator
    : ArtistGenreCommandValidatorBase<SetArtistGenreStatusCommand>
{
    public SetArtistGenreStatusCommandValidator()
    {
        ApplyArtistGenreRules(x => x.ArtistId, x => x.GenreId);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid ArtistGenreStatus value.");
    }
}
