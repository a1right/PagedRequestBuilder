using PagedRequestBuilder.Models;
using System.Linq;

namespace PagedRequestBuilder.Extensions;

public static class QueryableExtensionsPublic
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int size, int page)
    {
        if (size <= 0 || page < 1)
            return Enumerable.Empty<T>().AsAsyncQueryable();

        return query
            .Skip((size * (page - 1)))
            .Take(size);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequestBase request)
    {
        if (request.Size <= 0 || request.Page < 1)
            return Enumerable.Empty<T>().AsAsyncQueryable();

        return query
            .Skip((request.Size * (request.Page - 1)))
            .Take(request.Size);
    }
}
