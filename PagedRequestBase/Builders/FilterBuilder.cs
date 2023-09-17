using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Common.ValueParser.Models;
using PagedRequestBuilder.Constants;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders;

public class FilterBuilder<T> : IFilterBuilder<T> where T : class
{
    private readonly IPagedRequestValueParser _valueParser;
    private readonly IRequestPropertyMapper _propertyMapper;
    private readonly IQueryFilterCache<T> _queryFilterCache;
    private readonly IMethodCallExpressionBuilder _methodCallExpressionBuilder;

    public FilterBuilder(
        IPagedRequestValueParser valueParser,
        IRequestPropertyMapper propertyMapper,
        IQueryFilterCache<T> queryFilterCache,
        IMethodCallExpressionBuilder methodCallExpressionBuilder)
    {
        _valueParser = valueParser;
        _propertyMapper = propertyMapper;
        _queryFilterCache = queryFilterCache;
        _methodCallExpressionBuilder = methodCallExpressionBuilder;
    }
    public IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase request)
    {
        var simpleFilters = BuildSimpleFilters(request.Filters);
        var complexFilters = BuildComplexFilters(request.ComplexFilters);

        return simpleFilters.Concat(complexFilters);
    }

    private IEnumerable<QueryFilter<T>> BuildSimpleFilters(IEnumerable<FilterEntry> entries)
    {
        foreach (var filter in entries)
        {
            if (filter is null)
                continue;

            var cached = _queryFilterCache.Get(filter);
            if (cached is not null)
            {
                yield return (QueryFilter<T>)cached;
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

    private IEnumerable<QueryFilter<T>> BuildComplexFilters(IEnumerable<IEnumerable<FilterEntry>> entries)
    {
        foreach (var complexFilter in entries)
        {
            QueryFilter<T>? aggregate = null;

            foreach (var filter in BuildSimpleFilters(complexFilter))
                aggregate |= filter;

            if (aggregate is not null)
                yield return aggregate;
        }
    }

    private Expression<Func<T, bool>>? GetPredicate(FilterEntry entry)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertySelector = GetPropertySelector(entry.Property, parameter);
            var assignablePropertyType = propertySelector.Type;

            var providedValue = _valueParser.GetValue(entry.Value, assignablePropertyType);

            var constant = Expression.Constant(providedValue, typeof(ValueParseResult));
            var constantClojure = Expression.Property(constant, nameof(ValueParseResult.Value));
            var converted = Expression.Convert(constantClojure, assignablePropertyType);
            if (assignablePropertyType.IsEnum || Nullable.GetUnderlyingType(assignablePropertyType).IsEnum)
            {
                converted = Expression.Convert(converted, typeof(int));
                propertySelector = Expression.Convert(propertySelector, typeof(int));
            }

            var newExpression = GetOperationExpression(propertySelector, converted, entry.Operation, assignablePropertyType);
            return Expression.Lambda<Func<T, bool>>(newExpression, parameter);
        }
        catch (Exception ex)
        {
            if (PaginationSetting.ThrowExceptions)
                throw;

            return null;
        }
    }

    private MemberExpression GetPropertySelector(string propertyKeys, Expression parameter)
    {
        var keys = propertyKeys.Split('.');
        var propertySelector = parameter;
        var assignablePropertyType = typeof(T);

        foreach (var property in keys)
        {
            var typePropertyName = _propertyMapper.MapRequestNameToPropertyName(property, assignablePropertyType);
            propertySelector = Expression.PropertyOrField(propertySelector, typePropertyName);
            assignablePropertyType = propertySelector.Type;
        }

        if (propertySelector is null)
            throw new ArgumentNullException(nameof(propertySelector));

        return (MemberExpression)propertySelector;
    }

    private Expression GetOperationExpression(Expression left, Expression right, string? operation, Type assignablePropertyType) => operation switch
    {
        Strings.RequestOperations.Equal => Expression.Equal(left, right),
        Strings.RequestOperations.GreaterThen => Expression.GreaterThan(left, right),
        Strings.RequestOperations.GreaterThenOrEquals => Expression.GreaterThanOrEqual(left, right),
        Strings.RequestOperations.LessThen => Expression.LessThan(left, right),
        Strings.RequestOperations.LessThenOrEqual => Expression.LessThanOrEqual(left, right),
        Strings.RequestOperations.NotEqual => Expression.NotEqual(left, right),
        Strings.RequestOperations.Contains => _methodCallExpressionBuilder.Build(Strings.MethodInfoNames.Contains, left, right, assignablePropertyType),
        Strings.RequestOperations.In => _methodCallExpressionBuilder.Build(Strings.MethodInfoNames.Contains, right, left, right.Type),

        _ => throw new NotImplementedException()
    };
}

public interface IFilterBuilder<T> where T : class
{
    IEnumerable<IQueryFilter<T>> BuildFilters(PagedRequestBase request);
}
