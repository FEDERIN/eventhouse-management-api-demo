using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Exceptions;
using MediatR;

namespace EventHouse.Management.Application.Commands.Genres.Delete;

internal sealed class DeleteGenreCommandHandler(IGenreRepository repository)
       : IRequestHandler<DeleteGenreCommand>
{
    private readonly IGenreRepository _repository = repository;

    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (result is false)
            throw new NotFoundException("Genre", request.Id);
    }
}
