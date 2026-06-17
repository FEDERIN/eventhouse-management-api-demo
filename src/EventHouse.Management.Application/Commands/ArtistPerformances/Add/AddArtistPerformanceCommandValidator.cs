using FluentValidation;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Add;

internal sealed class AddArtistPerformanceCommandValidator
    : ArtistPerformanceCommandValidatorBase<AddArtistPerformanceCommand>
{
    public AddArtistPerformanceCommandValidator()
    {
        RuleFor(x => x.EventVenueCalendarId)
            .NotEmpty().WithMessage("EventVenueCalendarId must be a non-empty GUID.");

        RuleFor(x => x.ArtistId)
            .NotEmpty().WithMessage("ArtistId must be a non-empty GUID.");

        ApplyArtistPerformanceRules(x => x.SetStart, x => x.SetEnd);
    }
}