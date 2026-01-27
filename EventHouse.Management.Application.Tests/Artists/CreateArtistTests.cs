using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.Common.Enums;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Domain.Entities;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Artists;

public sealed class CreateArtistCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateArtist_AndReturnDto()
    {
        // Arrange
        var repo = Substitute.For<IArtistRepository>();

        // AddAsync devuelve Task, así que devolvemos Task.CompletedTask
        repo.AddAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var handler = new CreateArtistCommandHandler(repo);

        var request = new CreateArtistCommand(
            Name: "  The Rolling Stones  ",
            Category: ArtistCategoryDto.Band // ajusta al enum de tu command
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert (DTO)
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("The Rolling Stones", result.Name); // Trim()
        Assert.Equal(request.Category, result.Category);

        // Assert (repo llamado)
        await repo.Received(1).AddAsync(
            Arg.Is<Artist>(a =>
                a.Id == result.Id &&
                a.Name == "The Rolling Stones"
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
