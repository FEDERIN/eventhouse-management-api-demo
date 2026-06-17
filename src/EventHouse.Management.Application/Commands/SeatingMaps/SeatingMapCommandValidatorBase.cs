
using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.SeatingMaps;

internal abstract class SeatingMapCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected SeatingMapCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplySeatingMapRules(
        Expression<Func<TCommand, string>> name,
        Expression<Func<TCommand, int>> version)
    {
        RuleFor(name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(version)
            .GreaterThan(0).WithMessage("Version must be greater than 0.");
    }
}