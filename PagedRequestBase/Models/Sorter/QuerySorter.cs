using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Sorter;

internal struct QuerySorter<T>
{
    public bool Descending { get; }
    public Expression<Func<T, object>> Sorter { get; }

    public QuerySorter(Expression<Func<T, object>> keySelector, bool descending = false)
    {
        Sorter = keySelector;
        Descending = descending;
    }
}
