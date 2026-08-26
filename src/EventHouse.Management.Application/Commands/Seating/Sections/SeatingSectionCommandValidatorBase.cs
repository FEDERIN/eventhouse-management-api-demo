using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Seating.Sections;

internal abstract class SeatingSectionCommandValidatorBase<TCommand>
    : AbstractValidator<TCommand>
{
    protected SeatingSectionCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplySeatingSectionRules(
        Expression<Func<TCommand, Guid>> seatingMapId)
    {
        RuleFor(seatingMapId)
            .NotEmpty()
            .WithMessage("SeatingMapId must be a non-empty GUID.");
    }
}