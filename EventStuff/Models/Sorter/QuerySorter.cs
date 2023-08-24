using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Sorter
{
    public class QuerySorter<T> : IQuerySorter<T>
    {
        private Expression<Func<T, object>> _keySelector;
        private readonly bool _descending;

        public bool Descending => _descending;
        public Expression<Func<T, object>> Sorter => _keySelector;

        public QuerySorter(Expression<Func<T, object>> keySelector, bool descending = false)
        {
            _keySelector = keySelector;
            _descending = descending;
        }
        public static implicit operator Expression<Func<T, object>>(QuerySorter<T> filter) => filter._keySelector;
    }
}
