using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete
{
    internal sealed class DeleteArtistCommandHandler(IVenueRepository repository)
           : IRequestHandler<DeleteVenueCommand>
    {
        public async Task Handle(DeleteVenueCommand request, CancellationToken ct)
        {
            var result = await repository.DeleteAsync(request.Id, ct);

            if (result is false)
                throw new NotFoundException("Venue", request.Id);
        }
    }
}