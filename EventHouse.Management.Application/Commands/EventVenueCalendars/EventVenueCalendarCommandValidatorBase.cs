using EventHouse.Management.Application.Common.Enums;
using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars;

internal abstract class EventVenueCalendarCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected EventVenueCalendarCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventVenueCalendarRules(
        Func<TCommand, DateTimeOffset> startDateSelector,
        Expression<Func<TCommand, DateTimeOffset?>> endDateExpression,
        Expression<Func<TCommand, EventVenueCalendarStatusDto>> status)
    {
        RuleFor(endDateExpression)
            .Must((cmd, end) => end is null || end.Value >= startDateSelector(cmd))
            .WithMessage("EndDate must be greater than or equal to StartDate.");

        RuleFor(status)
            .IsInEnum().WithMessage("The provided status is not valid.");
    }
}