using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Sorter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Builders;

public class SorterBuilder<T> : ISorterBuilder<T> where T : class
{
    private readonly IRequestPropertyMapper _propertyMapper;
    private readonly IQuerySorterCache<T> _querySorterCache;

    public SorterBuilder(IRequestPropertyMapper propertyMapper, IQuerySorterCache<T> querySorterCache)
    {
        _propertyMapper = propertyMapper;
        _querySorterCache = querySorterCache;
    }

    public IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase<T>? request)
    {
        if (request is not { Sorters: { Count: > 0 } })
            yield break;

        foreach (var sorter in request.Sorters)
        {
            if (sorter is null)
                continue;

            var cached = _querySorterCache.Get(sorter);
            if (cached is not null)
            {
                yield return cached;
                continue;
            }

            var keySelector = GetKeySelector(sorter);
            if (keySelector != null)
            {
                var querySorter = new QuerySorter<T>(keySelector, sorter.Descending ?? false);
                yield return querySorter;
                _querySorterCache.Set(sorter, querySorter);
            }
        }
    }

    private Expression<Func<T, object>>? GetKeySelector(SorterEntry entry)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var typePropertyName = _propertyMapper.MapRequestNameToPropertyName<T>(entry.Property);
            var propertySelector = Expression.PropertyOrField(parameter, typePropertyName);
            var propertyType = typeof(T).GetProperty(typePropertyName).PropertyType;

            if (entry.Nested is not null)
            {
                foreach (var nested in entry.Nested)
                {
                    typePropertyName = _propertyMapper.MapNestedRequestNameToPropertyName<T>(nested, propertyType);
                    propertySelector = Expression.PropertyOrField(propertySelector, typePropertyName);
                    propertyType = propertyType.GetProperty(typePropertyName).PropertyType;
                }
            }

            var converted = Expression.Convert(propertySelector, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, parameter);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

public interface ISorterBuilder<T> where T : class
{
    IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase<T>? request);
}
