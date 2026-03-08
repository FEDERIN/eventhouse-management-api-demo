using EventHouse.Management.Application.Common.Enums;
using FluentValidation;

namespace EventHouse.Management.Application.Commands.EventVenues;

internal abstract class EventVenueCommandValidatorBase <TCommand>
    : AbstractValidator<TCommand>
{
    protected EventVenueCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyEventVenueRules(
        Func<TCommand, Guid> eventId,
        Func<TCommand, EventVenueStatusDto> status)
    {
        RuleFor(x => status(x))
            .IsInEnum();
    }
}
