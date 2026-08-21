using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.EventVenueCalendars;

internal abstract class EventVenueCalendarCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected EventVenueCalendarCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventVenueCalendarDateRules(
        Func<TCommand, DateTimeOffset> startDateSelector,
        Expression<Func<TCommand, DateTimeOffset?>> endDateExpression)
    {
        RuleFor(endDateExpression)
            .Must((cmd, end) =>
                end is null ||
                end.Value >= startDateSelector(cmd))
            .WithMessage(
                "EndDate must be greater than or equal to StartDate.");
    }
}