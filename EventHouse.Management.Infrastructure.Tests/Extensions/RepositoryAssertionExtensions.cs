using FluentAssertions;
using FluentAssertions.Specialized;

namespace EventHouse.Management.Infrastructure.Tests.Extensions;

public static class RepositoryAssertionExtensions
{
    public static async Task<ExceptionAssertions<InvalidOperationException>> ShouldThrowDetachedException(
        this Func<Task> action)
    {
        return await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");
    }
}