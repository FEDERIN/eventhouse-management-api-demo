using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using MediatR;

namespace EventHouse.Management.Application.Commands.Venues.Delete
{
    internal sealed class DeleteArtistCommandHandler(IVenueRepository repository)
           : IRequestHandler<DeleteVenueCommand, DeleteResult>
    {
        private readonly IVenueRepository _repository = repository;

        public async Task<DeleteResult> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteAsync(request.Id, cancellationToken);

            if (result is false)
                return DeleteResult.NotFoundResult();

            return DeleteResult.Ok();
        }
    }
}
