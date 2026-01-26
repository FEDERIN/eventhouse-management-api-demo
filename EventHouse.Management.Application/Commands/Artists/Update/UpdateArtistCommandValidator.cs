using FluentValidation;

namespace EventHouse.Management.Application.Commands.Artists.Update;

internal sealed class UpdateArtistCommandValidator
    : ArtistCommandValidatorBase<UpdateArtistCommand>
{
    public UpdateArtistCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must be a non-empty GUID.");

        ApplyArtistRules(
            x => x.Name,
            x => x.Category
        );
    }
}