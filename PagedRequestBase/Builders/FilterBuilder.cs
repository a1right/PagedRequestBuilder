using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Common.ValueParser.Models;
using PagedRequestBuilder.Constants;
using PagedRequestBuilder.Infrastructure.Exceptions;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Filter;
using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders;

internal class FilterBuilder<T> : IFilterBuilder<T> where T : class
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

    public QueryFilter<T>[] BuildFilters(ref PagedRequestBase? request)
    {
        if (request is null)
            return Array.Empty<QueryFilter<T>>();

        var result = new QueryFilter<T>[request.Value.Filters.Length + request.Value.ComplexFilters.Length];
        var simpleFilters = BuildSimpleFilters(request.Value.Filters, result);
        var complexFilters = BuildComplexFilters(request.Value.ComplexFilters, result, simpleFilters.Length);

        return result;
    }

    private QueryFilter<T>[] BuildSimpleFilters(FilterEntry[] entries, QueryFilter<T>[] result)
    {
        for (var index = 0; index < entries.Length; index++)
        {
            var filter = entries[index];
            var cached = _queryFilterCache.Get(ref entries[index]);
            if (cached is not null)
            {
                result[index] = cached.Value;
                continue;
            }

            var predicate = GetPredicate(ref filter);

            if (predicate != null)
            {
                var queryFilter = new QueryFilter<T>(predicate);
                result[index] = queryFilter;
                _queryFilterCache.Set(ref filter, ref queryFilter);
            }
        }

        return result;
    }

    private QueryFilter<T>[] BuildComplexFilters(FilterEntry[][] entries, QueryFilter<T>[] result, int startIndex = 0)
    {
        for (var index = 0; index < entries.Length; index++)
        {
            QueryFilter<T>? aggregate = null;
            foreach (var filter in BuildSimpleFilters(entries[index], new QueryFilter<T>[entries[index].Length]))
                aggregate |= filter;

            if (aggregate is not null)
                result[startIndex++] = aggregate.Value;
        }

        return result;
    }

    private Expression<Func<T, bool>>? GetPredicate(ref FilterEntry entry)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertySelector = GetPropertySelector(entry.Property, parameter);
            var assignablePropertyType = propertySelector.Type;

            var providedValue = _valueParser.GetValue(entry.Value, assignablePropertyType);

            var constant = Expression.Constant(providedValue, typeof(ValueParseResult));
            var constantClojure = Expression.Property(constant, nameof(ValueParseResult.Value));
            var converted = Expression.Convert(constantClojure, providedValue.ValueType);
            if (assignablePropertyType.IsEnum || Nullable.GetUnderlyingType(assignablePropertyType) != null && Nullable.GetUnderlyingType(assignablePropertyType).IsEnum)
            {
                HandleEnum(ref propertySelector, ref converted, entry.Operation);
            }

            var newExpression = GetOperationExpression(propertySelector, converted, entry.Operation, assignablePropertyType);
            return Expression.Lambda<Func<T, bool>>(newExpression, parameter);
        }
        catch (PagedRequestBuilderException ex) when (ex is OperationNotImplementedException operationException)
        {
            throw new OperationNotImplementedException($"Operation '{entry.Operation}' is not supported for property '{entry.Property}' or provided value '{entry.Value}' is invalid");
        }
    }

    private void HandleEnum(ref Expression propertySelector, ref UnaryExpression convertedValue, string operation)
    {
        if (operation is Strings.RequestOperations.In or Strings.RequestOperations.Contains)
            return;

        convertedValue = Expression.Convert(convertedValue, typeof(int));
        propertySelector = Expression.Convert(propertySelector, typeof(int));

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

        _ => throw new OperationNotImplementedException()
    };
}

internal interface IFilterBuilder<T> where T : class
{
    QueryFilter<T>[] BuildFilters(ref PagedRequestBase? request);
}
