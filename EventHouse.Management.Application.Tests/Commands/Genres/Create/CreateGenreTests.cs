
using EventHouse.Management.Application.Commands.Genres.Create;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Commands.Genres.Create;

public sealed class CreateGenreTests
{
    [Fact]
    public async Task Handle_ShouldCreateGenre_AndReturnDto()
    {
        // Arrange
        var repo = Substitute.For<IGenreRepository>();
        repo.AddAsync(Arg.Any<Genre>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var handler = new CreateGenreCommandHandler(repo);

        var request = new CreateGenreCommand(
            Name: "Rock"
        );

        var ct = new CancellationTokenSource().Token;

        // Act
        var result = await handler.Handle(request, ct);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(request.Name, result.Name);

        await repo.Received(1).AddAsync(
            Arg.Is<Genre>(e =>
                e.Id == result.Id &&
                e.Name == request.Name
            ),
            ct
        );
    }
}
