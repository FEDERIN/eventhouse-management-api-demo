using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists;

internal abstract class ArtistCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected ArtistCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyArtistRules(
        Func<TCommand, string> name,
        Func<TCommand, ArtistCategoryDto> category)
    {
        RuleFor(x => name(x))
            .NotEmpty().WithMessage("Name is require.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => category(x))
            .IsInEnum();
    }
}
