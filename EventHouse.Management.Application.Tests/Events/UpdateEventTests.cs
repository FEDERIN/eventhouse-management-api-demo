using EventHouse.Management.Application.Commands.Events.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using EventScopeDto = EventHouse.Management.Application.Common.Enums.EventScope;

namespace EventHouse.Management.Application.Tests.Events;

public sealed class UpdateEventTests
{
    [Fact]
    public async Task Handle_WhenEventExists_ShouldUpdateAndReturnUpdated()
    {
        var repo = Substitute.For<IEventRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        var entity = new Event(id, "Summer Fest 2026", "Annual open-air music festival.", EventScope.Local);

        repo.GetByIdAsync(id, ct).Returns(entity);
        repo.UpdateAsync(Arg.Any<Event>(), ct).Returns(Task.CompletedTask);

        var handler = new UpdateEventCommandHandler(repo);

        var cmd = new UpdateEventCommand(
            Id: id,
            Name: "Summer Fest 2027",
            Description: "Annual open-air music festival and Comedy.",
            Scope: EventScopeDto.International
        );

        var result = await handler.Handle(cmd, ct);

        Assert.Equal(UpdateResult.Success, result);

        await repo.Received(1).UpdateAsync(
            Arg.Is<Event>(e =>
                e.Id == id &&
                e.Name == "Summer Fest 2027" &&
                e.Description == "Annual open-air music festival and Comedy." &&
                e.Scope == EventScope.International
            ),
            ct
        );
    }

}
