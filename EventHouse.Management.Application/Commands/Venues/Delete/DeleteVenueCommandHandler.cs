using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete
{
    internal sealed class DeleteArtistCommandHandler(IVenueRepository repository)
           : IRequestHandler<DeleteVenueCommand>
    {
        private readonly IVenueRepository _repository = repository;

        public async Task Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteAsync(request.Id, cancellationToken);

            if (result is false)
                throw new NotFoundException("Venue", request.Id);
        }
    }
}
