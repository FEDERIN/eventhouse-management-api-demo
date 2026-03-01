using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Domain.Tests.Entities;

public class ArtistGenreTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenArtistIdIsEmpty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new ArtistGenre(Guid.Empty, Guid.NewGuid(), ArtistGenreStatus.Active, true));

        Assert.Equal("artistId", ex.ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenGenreIdIsEmpty()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        new ArtistGenre(Guid.NewGuid(), Guid.Empty, ArtistGenreStatus.Active, true));

        Assert.Equal("genreId", ex.ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenStatusIsInvalid()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        new ArtistGenre(Guid.NewGuid(), Guid.NewGuid(), (ArtistGenreStatus)999, true));

        Assert.Equal("status", ex.ParamName);
    }

    [Fact]
    public void SetStatus_ShouldThrowArgumentOutOfRangeException_WhenStatusIsInvalid()
    {
        // Arrange
        var artistGenre = CreateValidArtistGenre();
        var invalidStatus = (ArtistGenreStatus)999;

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        artistGenre.SetStatus(invalidStatus));

        Assert.Equal("status", ex.ParamName);
    }

    // Helper para crear una instancia válida para los tests de métodos
    private static ArtistGenre CreateValidArtistGenre()
    {
        return new ArtistGenre(Guid.NewGuid(), Guid.NewGuid(), ArtistGenreStatus.Active, false);
    }
}