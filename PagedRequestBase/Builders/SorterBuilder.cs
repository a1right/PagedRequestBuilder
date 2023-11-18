using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Sorter;
using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders;

internal class SorterBuilder<T> : ISorterBuilder<T> where T : class
{
    private readonly IRequestPropertyMapper _propertyMapper;
    private readonly IQuerySorterCache<T> _querySorterCache;

    public SorterBuilder(IRequestPropertyMapper propertyMapper, IQuerySorterCache<T> querySorterCache)
    {
        _propertyMapper = propertyMapper;
        _querySorterCache = querySorterCache;
    }

    public QuerySorter<T>[] BuildSorters(ref PagedRequestBase? request)
    {
        if (request is null)
            return Array.Empty<QuerySorter<T>>();

        var result = new QuerySorter<T>[request.Value.Sorters.Length + request.Value.SortKeys.Length];
        var sortersByEntry = BuildSortersFromEntries(request.Value.Sorters, result);
        var sortersByKeys = BuildSortersFromKeys(request.Value.SortKeys, result, request.Value.Sorters.Length);

        return result;
    }

    private QuerySorter<T>[] BuildSortersFromEntries(SorterEntry[] entries, QuerySorter<T>[] result, int startIndex = 0)
    {
        for (var index = 0; index < entries.Length; index++)
        {
            var cached = _querySorterCache.Get(ref entries[index]);
            if (cached is not null)
            {
                result[startIndex++] = cached.Value;
                continue;
            }

            var keySelector = GetKeySelector(ref entries[index]);
            if (keySelector != null)
            {
                var querySorter = new QuerySorter<T>(keySelector, entries[index].Descending);
                result[startIndex++] = querySorter;
                _querySorterCache.Set(ref entries[index], ref querySorter);
            }
        }

        return result;
    }

    private QuerySorter<T>[] BuildSortersFromKeys(string[] sortKeys, QuerySorter<T>[] result, int startIndex = 0)
    {
        var sorters = new SorterEntry[sortKeys.Length];
        for (var index = 0; index < sortKeys.Length; index++)
        {
            if (string.IsNullOrEmpty(sortKeys[index]))
                continue;

            var isDescending = sortKeys[index].StartsWith("-");

            sorters[index] = new SorterEntry()
            {
                Property = isDescending ? sortKeys[index][1..] : sortKeys[index],
                Descending = isDescending
            };

        }

        return BuildSortersFromEntries(sorters, result, startIndex);
    }

    private Expression<Func<T, object>>? GetKeySelector(ref SorterEntry entry)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertySelector = GetPropertySelector(entry.Property, parameter);

        var converted = Expression.Convert(propertySelector, typeof(object));
        return Expression.Lambda<Func<T, object>>(converted, parameter);
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
}

internal interface ISorterBuilder<T> where T : class
{
    QuerySorter<T>[] BuildSorters(ref PagedRequestBase? request);
}
