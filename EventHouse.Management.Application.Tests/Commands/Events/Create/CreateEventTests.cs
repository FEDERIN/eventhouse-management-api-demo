using EventHouse.Management.Application.Commands.Events.Create;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Commands.Events.Create;

public sealed class CreateEventTests
{
    [Fact]
    public async Task Handle_ShouldCreateEvent_AndReturnDto()
    {
        // Arrange
        var repo = Substitute.For<IEventRepository>();
        repo.AddAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var handler = new CreateEventCommandHandler(repo);

        var request = new CreateEventCommand(
            Name: "Summer Fest 2026",
            Description: "Annual open-air music festival.",
            Scope: EventScopeDto.Local
        );

        var ct = new CancellationTokenSource().Token;

        // Act
        var result = await handler.Handle(request, ct);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Scope, result.Scope);

        await repo.Received(1).AddAsync(
            Arg.Is<Event>(e =>
                e.Id == result.Id &&
                e.Name == request.Name &&
                e.Description == request.Description
            ),
            ct
        );
    }

}