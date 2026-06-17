using FluentValidation;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Commands.ArtistPerformances;

internal abstract class ArtistPerformanceCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
{
    protected ArtistPerformanceCommandValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ApplyArtistPerformanceRules(
        Expression<Func<TCommand, DateTimeOffset?>> startExpression,
        Expression<Func<TCommand, DateTimeOffset?>> endExpression)
    {
        var getStart = startExpression.Compile();
        var getEnd = endExpression.Compile();

        RuleFor(x => x)
            .Must(cmd =>
            {
                var start = getStart(cmd);
                var end = getEnd(cmd);

                if (!start.HasValue && !end.HasValue)
                {
                    return true;
                }

                if (start.HasValue != end.HasValue)
                {
                    return false;
                }

                return end!.Value >= start!.Value;
            })
            .WithMessage("SetEnd must be greater than or equal to SetStart when both are specified, and both must be provided if either is.");
    }
}