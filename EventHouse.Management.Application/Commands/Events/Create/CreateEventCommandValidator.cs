
namespace EventHouse.Management.Application.Commands.Events.Create;

public sealed class CreateEventCommandValidator
    : EventCommandValidatorBase<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        ApplyEventRules(
            x => x.Name,
            x => x.Description,
            x => x.Scope
        );
    }
}
