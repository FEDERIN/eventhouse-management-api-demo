using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.Events;

internal abstract class EventCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected EventCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventRules(
        Func<TCommand, string> name,
        Func<TCommand, string?> description,
        Func<TCommand, EventScopeDto> scope)
    {
        RuleFor(x => name(x))
            .NotEmpty().WithMessage("Name is require.")
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => description(x))
            .Must(d => d is null || d.Trim().Length > 0)
            .WithMessage("Description cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(x => scope(x))
            .IsInEnum();
    }
}
