using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using ArtistCategory = EventHouse.Management.Application.Common.Enums.ArtistCategory;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class UpdateArtistTests
{
    [Fact]
    public async Task Handle_WhenArtistExists_ShouldUpdateAndReturnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        var entity = new Artist(id, "Old", Domain.Enums.ArtistCategory.Band);

        repo.GetTrackedByIdAsync(id, ct).Returns(entity);
        repo.UpdateAsync(Arg.Any<Artist>(), ct).Returns(Task.CompletedTask);

        var handler = new UpdateArtistCommandHandler(repo);

        var cmd = new UpdateArtistCommand(
            Id: id,
            Name: " New Name ",
            Category: ArtistCategory.Singer
        );

        // Act
        var result = await handler.Handle(cmd, ct);

        // Assert
        Assert.Equal(UpdateResult.Success, result);

        await repo.Received(1).UpdateAsync(
            Arg.Is<Artist>(a =>
                a.Id == id &&
                a.Name == "New Name" &&                 // trim aplicado por entity.Update(...)
                a.Category == Domain.Enums.ArtistCategory.Singer      // mapeo DTO -> Domain aplicado
            ),
            ct
        );
    }

    [Fact]
    public async Task Handle_WhenArtistDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();
        var ct = new CancellationTokenSource().Token;

        repo.GetTrackedByIdAsync(id, ct).Returns((Artist?)null);

        var handler = new UpdateArtistCommandHandler(repo);

        var cmd = new UpdateArtistCommand(
            Id: id,
            Name: "Name",
            Category: ArtistCategory.Band
        );

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(cmd, ct));

        await repo.DidNotReceive()
            .UpdateAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }
}
