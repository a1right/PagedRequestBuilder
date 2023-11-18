using PagedRequestBuilder.Extensions;
using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Filter;

internal struct QueryFilter<T>
{
    public Expression<Func<T, bool>> Filter { get; }

    public QueryFilter(Expression<Func<T, bool>> filterExpression)
    {
        Filter = filterExpression;
    }
    public static QueryFilter<T>? operator |(QueryFilter<T>? left, QueryFilter<T>? right)
    {
        if (right is null)
            return left;

        if (left is null)
            return right;

        return new QueryFilter<T>(left.Value.Filter.OrElse(right.Value.Filter));
    }

    public static QueryFilter<T>? operator &(QueryFilter<T>? left, QueryFilter<T>? right)
    {
        if (right is null)
            return left;

        if (left is null)
            return right;

        return new QueryFilter<T>(left.Value.Filter.AndAlso(right.Value.Filter));
    }
}
