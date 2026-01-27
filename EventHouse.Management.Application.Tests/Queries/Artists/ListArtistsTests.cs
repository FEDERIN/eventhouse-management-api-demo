using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Queries.Artists;

public sealed class ListArtistsTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedResult()
    {
        // Arrange
        var repo = Substitute.For<IArtistRepository>();

        var query = new GetAllArtistsQuery
        {
            SortBy = ArtistSortField.Name,
            SortDirection = SortDirection.Asc,
            Page = 1,
            PageSize = 20
        };

        var items = new[]
        {
            new Artist(Guid.NewGuid(), "A", ArtistCategory.Band),
            new Artist(Guid.NewGuid(), "B", ArtistCategory.Singer)
        };

        var pagedResult = new PagedResultDto<Artist>
        {
            Items = items,
            TotalCount = 2,
            Page = 1,
            PageSize = 20
        };

        repo.GetPagedAsync(
                Arg.Any<ArtistQueryCriteria>(),
                Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var handler = new GetAllArtistsQueryHandler(repo);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.PageSize);
        Assert.Equal(2, result.Items.Count);

        await repo.Received(1)
            .GetPagedAsync(Arg.Any<ArtistQueryCriteria>(), Arg.Any<CancellationToken>());
    }
}
