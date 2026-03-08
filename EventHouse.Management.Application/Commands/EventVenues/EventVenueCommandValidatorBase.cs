using System.Linq.Expressions;
using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenues;

internal abstract class EventVenueCommandValidatorBase<TCommand>
    : AbstractValidator<TCommand>
{
    protected EventVenueCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventVenueRules(
        Expression<Func<TCommand, EventVenueStatusDto>> statusSelector)
    {
        RuleFor(statusSelector)
            .IsInEnum()
            .WithMessage("The provided status is not valid.");
    }
}