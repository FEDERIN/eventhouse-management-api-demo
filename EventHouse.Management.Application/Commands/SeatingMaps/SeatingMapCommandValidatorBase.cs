using FluentValidation;

namespace EventHouse.Management.Application.Commands.SeatingMaps;

internal abstract class SeatingMapCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected SeatingMapCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplySeatingMapRules(
        Func<TCommand, string> name,
        Func<TCommand, int> version)
    {
        RuleFor(x => name(x))
            .NotEmpty().WithMessage("Name is require.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);
        
        RuleFor(x => version(x))
            .GreaterThan(0).WithMessage("Version must be greater than 0.");
    }
}
