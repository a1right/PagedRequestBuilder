using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Filter;

internal class QueryFilter<T> : IQueryFilter<T>
{
    public Expression<Func<T, bool>> Filter { get; }

    public QueryFilter(Expression<Func<T, bool>> filterExpression)
    {
        Filter = filterExpression;
    }
}
