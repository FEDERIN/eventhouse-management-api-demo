using EventHouse.Management.Application.Commands.Events.Delete;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Events;

public sealed class DeleteEventTests
{
    [Fact]
    public async Task Handle_WhenEventExists_ShouldDelete_AndReturnOk()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();

        // El handler llama GetByIdAsync, no ExistsAsync
        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Event(id, "Summer Fest 2026", "Annual open-air music festival.", EventScope.Local));

        // DeleteAsync devuelve bool en tu handler
        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.Ok, result.Status);

        await repo.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnNotFound_AndNotCallDelete()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Event?)null);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.NotFound, result.Status);

        await repo.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
        await repo.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenGetByIdReturnsEntity_ButDeleteReturnsFalse_ShouldReturnNotFound()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Event(id, "Summer Fest 2026", "Annual open-air music festival.", EventScope.Local));

        repo.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new DeleteEventCommandHandler(repo);
        var cmd = new DeleteEventCommand(id);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(DeleteStatus.NotFound, result.Status);

        await repo.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }
}
