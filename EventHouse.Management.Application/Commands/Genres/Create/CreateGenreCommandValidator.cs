using FluentValidation;

namespace EventHouse.Management.Application.Commands.Genres.Create;

internal sealed class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}