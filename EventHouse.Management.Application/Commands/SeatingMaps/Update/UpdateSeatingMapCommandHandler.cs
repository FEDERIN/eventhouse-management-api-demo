using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.SeatingMaps.Update;

internal class UpdateSeatingMapCommandHandler(ISeatingMapRepository seatingMapRepository)
    : IRequestHandler<UpdateSeatingMapCommand>
{
    public async Task Handle(UpdateSeatingMapCommand request, CancellationToken cancellationToken)
    {
        var entity = await seatingMapRepository.GetTrackedByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("SeatingMap", request.Id);

        entity.Update(request.Name, request.Version, request.IsActive);
        await seatingMapRepository.UpdateAsync(entity, cancellationToken);
    }
}
