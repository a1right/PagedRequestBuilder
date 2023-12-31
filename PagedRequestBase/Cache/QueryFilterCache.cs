﻿using PagedRequestBuilder.Models.Filter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache;

internal class QueryFilterCache<T> : IQueryFilterCache<T>
{
    private readonly ConcurrentDictionary<FilterEntry, IQueryFilter<T>> _queryFilterCache = new();
    public IQueryFilter<T>? Get(FilterEntry entry)
    {
        _queryFilterCache.TryGetValue(entry, out var compiledFilter);
        return compiledFilter;
    }

    public void Set(FilterEntry entry, IQueryFilter<T> filter) => _queryFilterCache.TryAdd(entry, filter);
}

public interface IQueryFilterCache<T>
{
    IQueryFilter<T>? Get(FilterEntry entry);
    void Set(FilterEntry entry, IQueryFilter<T> filter);
}
