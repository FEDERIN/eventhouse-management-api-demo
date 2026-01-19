using FluentValidation;

namespace EventHouse.Management.Application.Commands.Genres.Update;

public sealed class UpdateGenreCommandValidator
    : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must be a non-empty GUID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is require.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot contain only whitespace.")
            .MaximumLength(200);
    }

}
