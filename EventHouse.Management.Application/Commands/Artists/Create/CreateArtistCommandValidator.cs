
namespace EventHouse.Management.Application.Commands.Artists.Create;

internal sealed class CreateArtistCommandValidator
    : ArtistCommandValidatorBase<CreateArtistCommand>
{
    public CreateArtistCommandValidator()
    {
        ApplyArtistRules(
            x => x.Name,
            x => x.Category
        );
    }
}
