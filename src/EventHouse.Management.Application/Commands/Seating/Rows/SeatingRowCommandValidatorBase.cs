using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Seating.Rows;

internal abstract class SeatingRowCommandValidatorBase<TCommand>
    : AbstractValidator<TCommand>
{
    protected SeatingRowCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplySeatingRowRules(
        Expression<Func<TCommand, Guid>> seatingMapId,
        Expression<Func<TCommand, Guid>> seatingSectionId)
    {
        RuleFor(seatingMapId)
            .NotEmpty()
            .WithMessage("SeatingMapId must be a non-empty GUID.");

        RuleFor(seatingSectionId)
            .NotEmpty()
            .WithMessage("SeatingSectionId must be a non-empty GUID.");
    }
}