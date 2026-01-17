using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using NSubstitute;

namespace EventHouse.Management.Application.Tests.Events;

public sealed class ListEventsTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedResult()
    {
        // Arrange
        var repo = Substitute.For<IEventRepository>();

        var query = new GetAllEventsQuery
        {
            SortBy = EventSortField.Name,
            SortDirection = SortDirection.Asc,
            Page = 1,
            PageSize = 20
        };

        var items = new[]
        {
            new Event(Guid.NewGuid(), "Summer Fest 2026", "Annual open-air music festival.", EventScope.Local),
            new Event(Guid.NewGuid(), "Summer Fest 2027", "Annual open-air music festival and Comedy.", EventScope.International)
        };

        var pagedResult = new PagedResultDto<Event>
        {
            Items = items,
            TotalCount = 2,
            Page = 1,
            PageSize = 20
        };

        repo.GetPagedAsync(
                Arg.Any<EventQueryCriteria>(),
                Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var handler = new GetAllEventsQueryHandler(repo);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.PageSize);
        Assert.Equal(2, result.Items.Count);

        await repo.Received(1)
            .GetPagedAsync(Arg.Any<EventQueryCriteria>(), Arg.Any<CancellationToken>());
    }
}
