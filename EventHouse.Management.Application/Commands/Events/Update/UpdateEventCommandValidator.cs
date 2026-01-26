using FluentValidation;

namespace EventHouse.Management.Application.Commands.Events.Update;

internal sealed class UpdateEventCommandValidator
    : EventCommandValidatorBase<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must be a non-empty GUID.");

        ApplyEventRules(
            x => x.Name,
            x => x.Description,
            x => x.Scope
        );
    }
}