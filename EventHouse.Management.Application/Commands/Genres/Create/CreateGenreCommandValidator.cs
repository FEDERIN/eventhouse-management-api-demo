using FluentValidation;

namespace EventHouse.Management.Application.Commands.Genres.Create;

public sealed class CreateGenreCommandValidator
    : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name es requerido.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name no puede ser solo espacios.")
            .MaximumLength(200);
    }

}
