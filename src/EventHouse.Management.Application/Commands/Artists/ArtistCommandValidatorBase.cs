using EventHouse.Management.Application.Common.Enums;
using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Artists;

internal abstract class ArtistCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected ArtistCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyArtistRules(
        Expression<Func<TCommand, string>> name,
        Expression<Func<TCommand, ArtistCategoryDto>> category)
    {
        RuleFor(name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(category)
            .IsInEnum().WithMessage("'Category' has a range of values which does not include the provided value.");
    }
}