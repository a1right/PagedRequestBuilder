using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Filter;

internal class QueryFilter<T> : IQueryFilter<T>
{
    private readonly Expression<Func<T, bool>> _filterExpression;
    public Expression<Func<T, bool>> Filter => _filterExpression;

    public QueryFilter(Expression<Func<T, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }
}
