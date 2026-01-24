using EventHouse.Management.Application.Commands.Events.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Events;

public sealed class DeleteEventTests
{
    [Fact]
    public async Task Handle_WhenEventExists_ShouldDeleteAndReturnOk()
    {
        // Arrange
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        repo.DeleteAsync(id, ct).Returns(true);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        // Act
        var result = await handler.Handle(cmd, ct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).DeleteAsync(id, ct);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        repo.DeleteAsync(id, ct).Returns(false);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

        await repo.Received(1).DeleteAsync(id, ct);
    }
}
