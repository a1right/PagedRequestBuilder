using PagedRequestBuilder.Models.Sorter;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache
{
    internal class QuerySorterCache<T> : IQuerySorterCache<T>
    {
        private ConcurrentDictionary<SorterEntry, IQuerySorter<T>> _queryFilterCache = new();
        public IQuerySorter<T>? Get(SorterEntry entry)
        {
            if (_queryFilterCache.TryGetValue(entry, out var compiledSorter))
                return compiledSorter;

            return null;
        }

        public void Set(SorterEntry entry, IQuerySorter<T> filter)
        {
            _queryFilterCache.AddOrUpdate(entry, filter, (key, oldValue) => filter);
        }
    }

    public interface IQuerySorterCache<T>
    {
        IQuerySorter<T>? Get(SorterEntry entry);
        void Set(SorterEntry entry, IQuerySorter<T> filter);
    }
}
