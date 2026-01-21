using EventHouse.Management.Application.Commands.Artists.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class DeleteArtistTests
{
    [Fact]
    public async Task Handle_WhenArtistExists_ShouldDelete_AndReturnOk()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        // DeleteAsync devuelve bool en tu handler
        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new DeleteArtistCommandHandler(repo);
        var cmd = new DeleteArtistCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenDeleteReturnsFalse_ShouldThrowNotFoundException()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new DeleteArtistCommandHandler(repo);
        var cmd = new DeleteArtistCommand(id);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(cmd, CancellationToken.None));

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

}
