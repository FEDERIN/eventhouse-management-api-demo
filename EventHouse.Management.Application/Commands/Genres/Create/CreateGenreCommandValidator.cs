using FluentValidation;

namespace EventHouse.Management.Application.Commands.Genres.Create;

public sealed class CreateGenreCommandValidator
    : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is require.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);
    }

}
