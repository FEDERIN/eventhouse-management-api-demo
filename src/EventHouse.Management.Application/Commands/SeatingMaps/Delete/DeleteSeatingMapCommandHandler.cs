using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Delete;

internal sealed class DeleteSeatingMapCommandHandler(ISeatingMapRepository seatingMapRepository) 
    : IRequestHandler<DeleteSeatingMapCommand>
{
    public async Task Handle(DeleteSeatingMapCommand request, CancellationToken ct)
    {
        var result = await seatingMapRepository.DeleteAsync(request.Id, ct);

        if (result is false)
            throw new NotFoundException("SeatingMap", request.Id);
    }
}