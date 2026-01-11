using EventHouse.Management.Application.Commands.Artists.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class DeleteArtistTests
{
    [Fact]
    public async Task Handle_WhenArtistExists_ShouldDelete_AndReturnOk()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        // El handler llama GetByIdAsync, no ExistsAsync
        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Artist(id, "A", ArtistCategory.Band));

        // DeleteAsync devuelve bool en tu handler
        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new DeleteArtistCommandHandler(repo);
        var cmd = new DeleteArtistCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenArtistDoesNotExist_ShouldReturnNotFound_AndNotCallDelete()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var handler = new DeleteArtistCommandHandler(repo);
        var cmd = new DeleteArtistCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.NotFound, result.Status);

        await repo.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
        await repo.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenGetByIdReturnsEntity_ButDeleteReturnsFalse_ShouldReturnNotFound()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Artist(id, "A", ArtistCategory.Band));

        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new DeleteArtistCommandHandler(repo);
        var cmd = new DeleteArtistCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.NotFound, result.Status);

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }
}
