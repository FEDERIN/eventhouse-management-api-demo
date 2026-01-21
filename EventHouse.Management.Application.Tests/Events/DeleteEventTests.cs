using EventHouse.Management.Application.Commands.Events.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Events;

public sealed class DeleteEventTests
{
    [Fact]
    public async Task Handle_WhenEventExists_ShouldDelete_AndReturnOk()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();

        // DeleteAsync devuelve bool en tu handler
        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenDeleteReturnsFalse_ShouldThrowNotFoundException()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();

        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(cmd, CancellationToken.None));

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

}
