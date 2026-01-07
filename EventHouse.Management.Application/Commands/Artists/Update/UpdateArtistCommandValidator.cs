using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists.Update;

public sealed class UpdateArtistCommandValidator
    : ArtistCommandValidatorBase<UpdateArtistCommand>
{
    public UpdateArtistCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id no puede ser Guid.Empty.");

        ApplyArtistRules(
            x => x.Name,
            x => x.Category
        );
    }
}