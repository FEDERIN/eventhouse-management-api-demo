using EventHouse.Management.Api.Contracts.Common;
using EventHouse.Management.Application.Common.Pagination;

namespace EventHouse.Management.Api.Mappers.Common;

public static class PagedResultMapper
{
    public static PagedResult<TOut> ToContract<TIn, TOut>(
        this PagedResultDto<TIn> paged,
        Func<TIn, TOut> mapItem,
        HttpRequest request
        )
    {
        return new PagedResult<TOut>
        {
            Items = [.. paged.Items.Select(mapItem)],
            Page = paged.Page,
            PageSize = paged.PageSize,
            TotalCount = paged.TotalCount,
            Links = paged.BuildLinks(request)
        };
    }
}
