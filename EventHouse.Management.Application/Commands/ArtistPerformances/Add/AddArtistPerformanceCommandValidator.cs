using FluentValidation;

namespace EventHouse.Management.Application.Commands.ArtistPerformances.Add;

internal sealed class AddArtistPerformanceCommandValidator
    : AbstractValidator<AddArtistPerformanceCommand>
{
    public AddArtistPerformanceCommandValidator()
    {
        RuleFor(x => x.EventVenueCalendarId)
            .NotEmpty().WithMessage("The EventVenueCalendarId cannot be empty.");

        RuleFor(x => x.ArtistId)
            .NotEmpty().WithMessage("The ArtistId cannot be empty.");

        RuleFor(x => x.SetEnd)
            .Must((cmd, end) => end is null || cmd.SetStart is null || end.Value >= cmd.SetStart.Value)
            .WithMessage("SetEnd must be greater than or equal to SetStart when both are specified.");
    }
}
