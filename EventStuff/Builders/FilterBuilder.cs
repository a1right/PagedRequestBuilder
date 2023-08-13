using EventStuff.Models;
using EventStuff.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EventStuff.Builders
{
    public class FilterBuilder<T> : IFilterBuilder<T> where T : class
    {
        private readonly IPagedRequestValueParser _valueParser;
        private readonly IPagedRequestPropertyMapper _propertyMapper;

        public FilterBuilder(IPagedRequestValueParser valueParser, IPagedRequestPropertyMapper propertyMapper)
        {
            _valueParser = valueParser;
            _propertyMapper = propertyMapper;
        }
        public IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase<T>? request)
        {
            if (request is null or { Filters: not { Count: > 0 } })
                yield break;

            foreach (var filter in request.Filters)
            {
                var predicate = GetPredicate(filter);

                if (predicate != null)
                    yield return new QueryFilter<T>(predicate);

                filter.Value.Dispose();
            }
        }

        private Expression<Func<T, bool>>? GetPredicate(FilterEntry entry)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var typePropertyName = _propertyMapper.MapRequestNameToPropertyName<T>(entry.Property);
                var propertySelector = Expression.PropertyOrField(parameter, typePropertyName);
                var assignablePropertyType = typeof(T).GetProperty(typePropertyName).PropertyType;
                var constant = Expression.Constant(_valueParser.GetValue(entry.Value, assignablePropertyType));
                var newExpression = GetOperationExpression(propertySelector, constant, entry.Operation);
                return Expression.Lambda<Func<T, bool>>(newExpression, parameter);
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        private Expression GetOperationExpression(MemberExpression left, ConstantExpression right, string operation) => operation switch
        {
            "=" => Expression.Equal(left, right),
            ">" => Expression.GreaterThan(left, right),
            ">=" => Expression.GreaterThanOrEqual(left, right),
            "<" => Expression.LessThan(left, right),
            "<=" => Expression.LessThanOrEqual(left, right),
            "!=" => Expression.NotEqual(left, right),
            "contains" => Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right),

            _ => throw new NotImplementedException()
        };

    }

    public interface IFilterBuilder<T> where T : class
    {
        IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase<T>? request);
    }
}
