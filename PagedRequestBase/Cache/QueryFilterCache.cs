using PagedRequestBuilder.Models.Filter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache
{
    internal class QueryFilterCache<T> : IQueryFilterCache<T>
    {
        private ConcurrentDictionary<FilterEntry, IQueryFilter<T>> _queryFilterCache = new();
        public IQueryFilter<T>? Get(FilterEntry entry)
        {
            if (_queryFilterCache.TryGetValue(entry, out var compiledFilter))
                return compiledFilter;

            return null;
        }

        public void Set(FilterEntry entry, IQueryFilter<T> filter)
        {
            _queryFilterCache.AddOrUpdate(entry, filter, (key, oldValue) => filter);
        }
    }

    public interface IQueryFilterCache<T>
    {
        IQueryFilter<T>? Get(FilterEntry entry);
        void Set(FilterEntry entry, IQueryFilter<T> filter);
    }
}
