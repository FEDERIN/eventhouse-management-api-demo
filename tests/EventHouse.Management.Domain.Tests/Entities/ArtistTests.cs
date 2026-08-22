using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;

namespace EventHouse.Management.Domain.Tests.Entities;

public class ArtistTests
{
    [Fact]
    public void Constructor_EmptyId_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Artist(Guid.Empty, "Artist Name", ArtistCategory.Host));

        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void Constructor_InvalidName_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        new Artist(Guid.NewGuid(), "", ArtistCategory.Dancer));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Update_InvalidName_ThrowsArgumentException()
    {
        var artist = new Artist(Guid.NewGuid(), "Valid Name", ArtistCategory.Influencer);

        var exception = Assert.Throws<ArgumentException>(() =>
                artist.Update("", ArtistCategory.Band));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void AddGenre_EmptyGenreId_ThrowsArgumentException()
    {
        var artist = new Artist(Guid.NewGuid(), "Valid Name", ArtistCategory.Influencer);

        var exception = Assert.Throws<ArgumentException>(() =>
        artist.AddGenre(Guid.Empty, ArtistGenreStatus.Active));

        Assert.Equal("genreId", exception.ParamName);
    }

    [Fact]
    public void RemoveGenre_WhenPrimaryIsRemoved_PromotesNextActiveGenreToPrimary()
    {
        // Arrange
        var artist = new Artist(Guid.NewGuid(), "Artist Name", ArtistCategory.Dancer);
        var primaryGenreId = Guid.NewGuid();
        var secondaryGenreId = Guid.NewGuid();

        artist.AddGenre(primaryGenreId, ArtistGenreStatus.Active, isPrimary: true);
        artist.AddGenre(secondaryGenreId, ArtistGenreStatus.Active, isPrimary: false);

         artist.RemoveGenre(primaryGenreId);

        // Assert:
        var promotedGenre = artist.Genres.First(g => g.GenreId == secondaryGenreId);
        Assert.True(promotedGenre.IsPrimary);
    }

    [Fact]
    public void SetGenreStatus_SameStatus_ReturnsFalse()
    {
        // Arrange
        var artist = new Artist(Guid.NewGuid(), "Artist Name", ArtistCategory.Band);
        var genreId = Guid.NewGuid();
        var status = ArtistGenreStatus.Active;

        artist.AddGenre(genreId, status);

        var result = artist.SetGenreStatus(genreId, status);

        // Assert
        Assert.False(result);
    }

    //[Fact]
    //public void SetPrimaryGenre_AlreadyPrimary_ReturnsFalse()
    //{
    //    // Arrange
    //    var artist = new Artist(Guid.NewGuid(), "Artist Name", ArtistCategory.Comedian);
    //    var genreId = Guid.NewGuid();

    //    artist.AddGenre(genreId, ArtistGenreStatus.Active, isPrimary: true);

    //    var result = artist.SetPrimaryGenre(genreId);

    //    // Assert
    //    Assert.False(result);
    //}
}