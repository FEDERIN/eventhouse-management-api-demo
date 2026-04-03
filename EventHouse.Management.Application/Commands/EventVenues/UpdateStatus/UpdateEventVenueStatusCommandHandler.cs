using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.EventVenues;
using MediatR;

namespace EventHouse.Management.Application.Commands.EventVenues.UpdateStatus;

internal sealed class UpdateEventVenueStatusCommandHandler(IEventVenueRepository repository)
    : IRequestHandler<UpdateEventVenueStatusCommand>
{
    public async Task Handle(UpdateEventVenueStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetTrackedByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException("EventVenue", request.Id);

        var hasChanged = entity.ChangeStatus(EventVenueStatusMapper.ToDomainRequired(request.Status));

        if (hasChanged)
            await repository.UpdateAsync(entity, cancellationToken);
    }
}