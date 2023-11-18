using PagedRequestBuilder.Models.Sorter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache;

internal class QuerySorterCache<T> : IQuerySorterCache<T>
{
    private readonly ConcurrentDictionary<SorterEntry, QuerySorter<T>?> _queryFilterCache;

    public QuerySorterCache()
    {
        _queryFilterCache = new(new SorterEntryEqualityComparer());
    }

    public QuerySorter<T>? Get(ref SorterEntry entry)
    {
        _queryFilterCache.TryGetValue(entry, out var compiledSorter);
        return compiledSorter;
    }

    public void Set(ref SorterEntry entry, ref QuerySorter<T> filter) => _queryFilterCache.TryAdd(entry, filter);
}

internal interface IQuerySorterCache<T>
{
    QuerySorter<T>? Get(ref SorterEntry entry);
    void Set(ref SorterEntry entry, ref QuerySorter<T> filter);
}
