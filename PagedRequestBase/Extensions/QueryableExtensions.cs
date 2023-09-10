using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Models.Sorter;
using System.Collections.Generic;
using System.Linq;

namespace PagedRequestBuilder.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> query, IEnumerable<IQueryFilter<T>> filters)
    {
        foreach (var filter in filters)
        {
            query = query.Where(filter.Filter);
        }

        return query;
    }

    public static IQueryable<T> Where<T>(this IQueryable<T> query, IQueryFilter<T> filter)
    {
        query = query.Where(filter.Filter);

        return query;
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IQuerySorter<T> sorter) => sorter.Descending
        ? query.OrderByDescending(sorter.Sorter)
        : query.OrderBy(sorter.Sorter);

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<IQuerySorter<T>> sorters)
    {
        var firstSort = true;
        IOrderedQueryable<T> orderedQuery;
        foreach (var sorter in sorters)
        {
            if (firstSort)
            {
                query = query.OrderBy(sorter);
                firstSort = false;
                continue;
            }

            orderedQuery = (IOrderedQueryable<T>)query;
            query = orderedQuery.ThenBy(sorter);
        }

        return query;
    }

    public static IQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, IQuerySorter<T> sorter)
    {
        return sorter.Descending ? query.ThenByDescending(sorter.Sorter) : query.ThenBy(sorter.Sorter);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int size, int page)
    {
        if (size <= 0 || page < 1)
            return Enumerable.Empty<T>().AsQueryable();

        return query
            .Skip((size * (page - 1)))
            .Take(size);
    }
}

public static class QueryableExtensionsPublic
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int size, int page)
    {
        if (size <= 0 || page < 1)
            return Enumerable.Empty<T>().AsQueryable();

        return query
            .Skip((size * (page - 1)))
            .Take(size);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequestBase request)
    {
        if (request.Size <= 0 || request.Page < 1)
            return Enumerable.Empty<T>().AsQueryable();

        return query
            .Skip((request.Size * (request.Page - 1)))
            .Take(request.Size);
    }
}
