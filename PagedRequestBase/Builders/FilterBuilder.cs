using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders
{
    public class FilterBuilder<T> : IFilterBuilder<T> where T : class
    {
        private readonly IPagedRequestValueParser _valueParser;
        private readonly IPagedRequestPropertyMapper _propertyMapper;
        private readonly IQueryFilterCache<T> _queryFilterCache;
        private readonly IMethodCallExpressionBuilder _methodCallExpressionBuilder;

        public FilterBuilder(
            IPagedRequestValueParser valueParser,
            IPagedRequestPropertyMapper propertyMapper,
            IQueryFilterCache<T> queryFilterCache,
            IMethodCallExpressionBuilder methodCallExpressionBuilder)
        {
            _valueParser = valueParser;
            _propertyMapper = propertyMapper;
            _queryFilterCache = queryFilterCache;
            _methodCallExpressionBuilder = methodCallExpressionBuilder;
        }
        public IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase<T>? request)
        {
            if (request is null or { Filters: not { Count: > 0 } })
                yield break;

            foreach (var filter in request.Filters)
            {
                var cached = _queryFilterCache.Get(filter);
                if (cached is not null)
                {
                    yield return cached;
                    continue;
                }

                var predicate = GetPredicate(filter);

                if (predicate != null)
                {
                    var queryFilter = new QueryFilter<T>(predicate);
                    yield return queryFilter;
                    _queryFilterCache.Set(filter, queryFilter);
                }
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
                var newExpression = GetOperationExpression(propertySelector, constant, entry.Operation, assignablePropertyType);
                return Expression.Lambda<Func<T, bool>>(newExpression, parameter);
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        private Expression GetOperationExpression(MemberExpression left, ConstantExpression right, string operation, Type assignablePropertyType)
        {
            return operation switch
            {
                "=" => Expression.Equal(left, right),
                ">" => Expression.GreaterThan(left, right),
                ">=" => Expression.GreaterThanOrEqual(left, right),
                "<" => Expression.LessThan(left, right),
                "<=" => Expression.LessThanOrEqual(left, right),
                "!=" => Expression.NotEqual(left, right),
                "contains" => _methodCallExpressionBuilder.Build("Contains", left, right, assignablePropertyType),

                _ => throw new NotImplementedException()
            };
        }

    }

    public interface IFilterBuilder<T> where T : class
    {
        IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase<T>? request);
    }
}
