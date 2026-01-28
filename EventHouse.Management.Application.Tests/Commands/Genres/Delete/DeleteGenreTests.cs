using EventHouse.Management.Application.Commands.Genres.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Delete;

public sealed class DeleteGenreTests
{
    [Fact]
    public async Task Handle_WhenGenreExists_ShouldDeleteAndReturnOk()
    {
        // Arrange
        var repo = Substitute.For<IGenreRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        repo.DeleteAsync(id, ct).Returns(true);

        var handler = new DeleteGenreCommandHandler(repo);
        var cmd = new DeleteGenreCommand(id);

        // Act
        var result = await handler.Handle(cmd, ct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).DeleteAsync(id, ct);
    }

    [Fact]
    public async Task Handle_WhenGenreDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var repo = Substitute.For<IGenreRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        repo.DeleteAsync(id, ct).Returns(false);

        var handler = new DeleteGenreCommandHandler(repo);
        var cmd = new DeleteGenreCommand(id);

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

        await repo.Received(1).DeleteAsync(id, ct);
    }
}
