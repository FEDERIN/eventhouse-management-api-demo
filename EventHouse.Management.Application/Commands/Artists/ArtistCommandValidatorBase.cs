using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists;

public abstract class ArtistCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected ArtistCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyArtistRules(
        Func<TCommand, string> name,
        Func<TCommand, ArtistCategory> category)
    {
        RuleFor(x => name(x))
            .NotEmpty().WithMessage("Name es requerido.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name no puede ser solo espacios.")
            .MaximumLength(200);

        RuleFor(x => category(x))
            .IsInEnum();
    }
}
