using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Artists;

internal abstract class ArtistGenreCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected ArtistGenreCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyArtistGenreRules(
        Expression<Func<TCommand, Guid>> artistId,
        Expression<Func<TCommand, Guid>> genreId)
    {
        RuleFor(artistId)
            .NotEmpty().WithMessage("ArtistId must be a non-empty GUID.");

        RuleFor(genreId)
            .NotEmpty().WithMessage("GenreId must be a non-empty GUID.");
    }
}