using EventHouse.Management.Application.Common.Enums;
using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Events;

internal abstract class EventCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected EventCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventRules(
        Expression<Func<TCommand, string>> nameExpression,
        Expression<Func<TCommand, string?>> descriptionExpression,
        Expression<Func<TCommand, EventScopeDto>> scopeExpression)
    {
        RuleFor(nameExpression)
            .NotEmpty().WithMessage("Name is required.")
            .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(descriptionExpression)
            .Must(d => d is null || d.Trim().Length > 0)
            .WithMessage("Description cannot contain only whitespace.")
            .MaximumLength(200);

        RuleFor(scopeExpression)
            .IsInEnum().WithMessage("The provided scope is not valid.");
    }
}