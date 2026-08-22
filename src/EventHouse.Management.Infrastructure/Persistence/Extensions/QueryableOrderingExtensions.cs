using EventHouse.Management.Application.Common.Sorting;
using System.Linq.Expressions;

namespace EventHouse.Management.Infrastructure.Persistence.Extensions;

internal static class QueryableOrderingExtensions
{
    public static IOrderedQueryable<T> OrderByDirection<T, TKey>(
        this IQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        SortDirection direction)
    {
        return direction == SortDirection.Asc
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }

    public static IOrderedQueryable<T> ThenByDirection<T, TKey>(
        this IOrderedQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        SortDirection direction)
    {
        return direction == SortDirection.Asc
            ? query.ThenBy(keySelector)
            : query.ThenByDescending(keySelector);
    }
}