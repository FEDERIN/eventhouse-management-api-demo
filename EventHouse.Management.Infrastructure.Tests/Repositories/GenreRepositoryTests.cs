using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Repositories;
using EventHouse.Management.Infrastructure.Tests.Persistence;
using FluentAssertions;

namespace EventHouse.Management.Infrastructure.Tests.Repositories;

public class GenreRepositoryTests : BasePersistenceTest
{
    private readonly GenreRepository _repository;

    public GenreRepositoryTests()
    {
        _repository = new GenreRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenEntityIsDetached()
    {
        // Arrange
        var genre = CreateValidGenre("Rock");

        // Act
        var act = async () => await _repository.UpdateAsync(genre, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByName_WhenPartialMatch()
    {
        // Arrange
        await SeedAsync(
            CreateValidGenre("Rock"),
            CreateValidGenre("Pop")
        );
        var criteria = new GenreQueryCriteria { Name = "oc" };
        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Name.Should().Be("Rock");
    }

    [Theory]
    [InlineData(GenreSortField.Name, SortDirection.Asc, "Rock")]
    [InlineData(GenreSortField.Name, SortDirection.Desc, "Vallenato")]
    [InlineData(null, SortDirection.Asc, "Rock")]
    [InlineData(null, SortDirection.Desc, "Vallenato")]
    public async Task GetPagedAsync_ShouldApplyCorrectSorting(
        GenreSortField? sortField,
        SortDirection direction,
        string expectedFirstName)
    {
         // Arrange
        await SeedAsync(
            CreateValidGenre("Rock"),
            CreateValidGenre("Vallenato")
        );
        var criteria = new GenreQueryCriteria { SortBy = sortField, SortDirection = direction };
        // Act
        var result = await _repository.GetPagedAsync(criteria, TestContext.Current.CancellationToken);
        // Assert
        result.Items[0].Name.Should().Be(expectedFirstName);
    }

    private static Genre CreateValidGenre(string name)
    {
        return new Genre(Guid.NewGuid(), name);
    }

}