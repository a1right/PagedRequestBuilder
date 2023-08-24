using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Filter
{
    public class QueryFilter<T> : IQueryFilter<T>
    {
        private readonly Expression<Func<T, bool>> _filterExpression;
        public Expression<Func<T, bool>> Filter => _filterExpression;

        public QueryFilter(Expression<Func<T, bool>> filterExpression)
        {
            _filterExpression = filterExpression;
        }
        public static implicit operator Expression<Func<T, bool>>(QueryFilter<T> filter) => filter._filterExpression;
        public static explicit operator QueryFilter<T>(Expression<Func<T, bool>> expression) => new QueryFilter<T>(expression);
    }
}
