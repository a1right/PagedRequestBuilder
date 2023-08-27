using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Sorter;

internal class QuerySorter<T> : IQuerySorter<T>
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
}
