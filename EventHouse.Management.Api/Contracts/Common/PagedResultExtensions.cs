using EventHouse.Management.Application.Common.Pagination;
using Microsoft.AspNetCore.WebUtilities;

namespace EventHouse.Management.Api.Contracts.Common;

public static class PagedResultExtensions
{
    public static PaginationLinks BuildLinks<T>(this PagedResultDto<T> result, HttpRequest request)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}";

        var queryParams = QueryHelpers
            .ParseQuery(request.QueryString.Value ?? string.Empty)
            .ToDictionary(x => x.Key, x => (string?)x.Value.ToString());

        string BuildUrl(int page)
        {
            queryParams["page"] = page.ToString();
            queryParams["pageSize"] = result.PageSize.ToString();

            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }

        return new PaginationLinks
        {
            Self = BuildUrl(result.Page),
            First = BuildUrl(1),
            Previous = result.HasPreviousPage ? BuildUrl(result.Page - 1) : null,
            Next = result.HasNextPage ? BuildUrl(result.Page + 1) : null,
            Last = result.TotalPages > 0 ? BuildUrl(result.TotalPages) : null
        };
    }

    public static PagedResultDto<T> WithLinks<T>(this PagedResultDto<T> result, HttpRequest request)
        => new()
        {
            Items = result.Items,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
            Links = result.BuildLinks(request)
        };
}
