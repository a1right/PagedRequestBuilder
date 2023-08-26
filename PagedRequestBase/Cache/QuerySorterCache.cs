using PagedRequestBuilder.Models.Sorter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache;

internal class QuerySorterCache<T> : IQuerySorterCache<T>
{
    private ConcurrentDictionary<SorterEntry, IQuerySorter<T>> _queryFilterCache = new();
    public IQuerySorter<T>? Get(SorterEntry entry)
    {
        _queryFilterCache.TryGetValue(entry, out var compiledSorter);
        return compiledSorter;
    }

    public void Set(SorterEntry entry, IQuerySorter<T> filter) => _queryFilterCache.TryAdd(entry, filter);
}

public interface IQuerySorterCache<T>
{
    IQuerySorter<T>? Get(SorterEntry entry);
    void Set(SorterEntry entry, IQuerySorter<T> filter);
}
