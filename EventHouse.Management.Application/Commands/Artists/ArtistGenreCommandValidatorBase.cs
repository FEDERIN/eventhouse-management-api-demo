using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists;

internal abstract class ArtistGenreCommandValidatorBase<TCommand>
    : AbstractValidator<TCommand>
{
    protected void ApplyArtistGenreRules(
        Func<TCommand, Guid> artistId,
        Func<TCommand, Guid> genreId)
    {
        RuleFor(x => artistId(x))
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ArtistId must be a non-empty GUID.");

        RuleFor(x => genreId(x))
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("GenreId must be a non-empty GUID.");
    }
}
