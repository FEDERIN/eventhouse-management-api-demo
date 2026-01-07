using FluentValidation;

namespace EventHouse.Management.Application.Commands.Events.Update;

public sealed class UpdateEventCommandValidator
    : EventCommandValidatorBase<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id no puede ser Guid.Empty.");

        ApplyEventRules(
            x => x.Name,
            x => x.Description,
            x => x.Scope
        );
    }
}