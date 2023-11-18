using PagedRequestBuilder.Infrastructure.AsyncSequence;
using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Models.Sorter;
using System.Collections.Generic;
using System.Linq;

namespace PagedRequestBuilder.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> query, QueryFilter<T>[] filters)
    {
        for (var index = 0; index < filters.Length; index++)
        {
            var filter = filters[index];
            query = query.Where(ref filter);
        }

        return query;
    }

    public static IQueryable<T> Where<T>(this IQueryable<T> query, ref QueryFilter<T> filter)
    {
        query = query.Where(filter.Filter);

        return query;
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, ref QuerySorter<T> sorter) => sorter.Descending
        ? query.OrderByDescending(sorter.Sorter)
        : query.OrderBy(sorter.Sorter);

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, QuerySorter<T>[] sorters)
    {
        var firstSort = true;
        IOrderedQueryable<T> orderedQuery;
        for (var index = 0; index < sorters.Length; index++)
        {
            var sorter = sorters[index];
            if (firstSort)
            {
                query = query.OrderBy(ref sorter);
                firstSort = false;
                continue;
            }

            orderedQuery = (IOrderedQueryable<T>)query;
            query = orderedQuery.ThenBy(ref sorter);
        }

        return query;
    }

    public static IQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, ref QuerySorter<T> sorter)
    {
        return sorter.Descending ? query.ThenByDescending(sorter.Sorter) : query.ThenBy(sorter.Sorter);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int size, int page)
    {
        if (size < 1 || page < 1)
            return Enumerable.Empty<T>().AsAsyncQueryable();

        return query
            .Skip((size * (page - 1)))
            .Take(size);
    }
    public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source) =>
        new AsyncQueryable<T>(source.AsQueryable());
}