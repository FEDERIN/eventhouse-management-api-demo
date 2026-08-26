using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.Seating.Seats;

internal abstract class SeatingSeatCommandValidatorBase<TCommand>
    : AbstractValidator<TCommand>
{
    protected SeatingSeatCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplySeatingSeatRules(
        Expression<Func<TCommand, Guid>> seatingMapId,
        Expression<Func<TCommand, Guid>> seatingSectionId,
        Expression<Func<TCommand, Guid>> seatingRowId)
    {
        RuleFor(seatingMapId)
            .NotEmpty()
            .WithMessage("SeatingMapId must be a non-empty GUID.");

        RuleFor(seatingSectionId)
            .NotEmpty()
            .WithMessage("SeatingSectionId must be a non-empty GUID.");

        RuleFor(seatingRowId)
            .NotEmpty()
            .WithMessage("SeatingRowId must be a non-empty GUID.");
    }
}