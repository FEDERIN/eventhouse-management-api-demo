
using EventHouse.Management.Application.Commands.Genres.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Update;

public sealed class UpdateGenreTests
{
    [Fact]
    public async Task Handle_WhenGenreExists_ShouldUpdateAndReturnUpdated()
    {
        var repo = Substitute.For<IGenreRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        var entity = new Genre(id, "Rock update command");

        repo.GetTrackedByIdAsync(id, ct).Returns(entity);
        repo.UpdateAsync(Arg.Any<Genre>(), ct).Returns(Task.CompletedTask);

        var handler = new UpdateGenreCommandHandler(repo);

        var cmd = new UpdateGenreCommand(
            Id: id,
            Name: "Rock update command 2"
        );

        var result = await handler.Handle(cmd, ct);

        Assert.Equal(UpdateResult.Success, result);

        await repo.Received(1).UpdateAsync(
            Arg.Is<Genre>(e =>
                e.Id == id &&
                e.Name == "Rock update command 2"
            ),
            ct
        );
    }
}
