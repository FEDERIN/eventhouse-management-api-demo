using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.Events;

public abstract class EventCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected EventCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventRules(
        Func<TCommand, string> name,
        Func<TCommand, string?> description,
        Func<TCommand, EventScope> scope)
    {
        RuleFor(x => name(x))
            .NotEmpty().WithMessage("Name es requerido.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name no puede ser solo espacios.")
            .MaximumLength(200);

        RuleFor(x => description(x))
            .Must(d => d is null || d.Trim().Length > 0)
            .WithMessage("Description no puede ser solo espacios.")
            .MaximumLength(200);

        RuleFor(x => scope(x))
            .IsInEnum();
    }
}
