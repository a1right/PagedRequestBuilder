using PagedRequestBuilder.Extensions;
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

    public static implicit operator Expression<Func<T, bool>>(QueryFilter<T> filter) => filter.Filter;
    public static implicit operator QueryFilter<T>(Expression<Func<T, bool>> expression) => new QueryFilter<T>(expression);
    public static QueryFilter<T>? operator |(QueryFilter<T>? left, QueryFilter<T>? right)
    {
        if (right is null)
            return left;

        if (left is null)
            return right;

        return new QueryFilter<T>(left.Filter.OrElse(right.Filter));
    }

    public static QueryFilter<T>? operator &(QueryFilter<T>? left, QueryFilter<T>? right)
    {
        if (right is null)
            return left;

        if (left is null)
            return right;

        return new QueryFilter<T>(left.Filter.AndAlso(right.Filter));
    }
}
