namespace EventHouse.Management.Application.Common.Pagination;

internal static class PagedResultExtensions
{
    public static PagedResultDto<TDestination> MapTo<TSource, TDestination>(
        this PagedResultDto<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TDestination>> mapFunc)
    {
        return new PagedResultDto<TDestination>
        {
            Items = [.. mapFunc(source.Items)],
            TotalCount = source.TotalCount,
            Page = source.Page,
            PageSize = source.PageSize
        };
    }
}