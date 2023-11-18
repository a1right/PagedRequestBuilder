using PagedRequestBuilder.Models.Filter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache;

internal class QueryFilterCache<T> : IQueryFilterCache<T>
{
    private readonly ConcurrentDictionary<FilterEntry, QueryFilter<T>?> _queryFilterCache;
    public QueryFilterCache()
    {
        _queryFilterCache = new(new FilterEqualityComparers());
    }
    public QueryFilter<T>? Get(ref FilterEntry entry)
    {
        _queryFilterCache.TryGetValue(entry, out var compiledFilter);
        return compiledFilter;
    }

    public void Set(ref FilterEntry entry, ref QueryFilter<T> filter) => _queryFilterCache.TryAdd(entry, filter);
}

internal interface IQueryFilterCache<T>
{
    QueryFilter<T>? Get(ref FilterEntry entry);
    void Set(ref FilterEntry entry, ref QueryFilter<T> filter);
}
