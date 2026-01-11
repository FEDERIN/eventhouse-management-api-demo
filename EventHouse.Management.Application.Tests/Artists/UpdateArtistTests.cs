using EventHouse.Management.Application.Commands.Artists.Update;
using EventHouse.Management.Application.Common;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;
using ArtistCategoryDto = EventHouse.Management.Application.Common.Enums.ArtistCategory;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class UpdateArtistTests
{
    [Fact]
    public async Task Handle_WhenArtistExists_ShouldUpdate()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        var entity = new Artist(id, "Old", ArtistCategory.Band);

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(entity);

        repo.UpdateAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var handler = new UpdateArtistCommandHandler(repo);

        var cmd = new UpdateArtistCommand(
            Id: id,
            Name: " New Name ",
            Category: ArtistCategoryDto.Singer
        );

        await handler.Handle(cmd, CancellationToken.None);

        await repo.Received(1).UpdateAsync(
            Arg.Is<Artist>(a => a.Id == id && a.Name == "New Name"),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task Handle_WhenArtistDoesNotExist_ShouldThrowNotFound()
    {
        var repo = Substitute.For<IArtistRepository>();
        var id = Guid.NewGuid();

        repo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var handler = new UpdateArtistCommandHandler(repo);

        var cmd = new UpdateArtistCommand(id, "Name", ArtistCategoryDto.Band);

        var result = await handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(UpdateResult.NotFound, result);

        await repo.DidNotReceive()
            .UpdateAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }
}
