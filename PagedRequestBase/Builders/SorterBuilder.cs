using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Sorter;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase? request)
    {
        if (request is not { Sorters: { Count: > 0 } } and not { SortKeys: { Count: > 0 } })
            return Enumerable.Empty<IQuerySorter<T>>();

        var sortersByEntry = BuildSortersFromEntries(request.Sorters);
        var sortersByKeys = BuildSortersFromKeys(request.SortKeys);

        return sortersByEntry.Concat(sortersByKeys);
    }

    private IEnumerable<QuerySorter<T>> BuildSortersFromEntries(IEnumerable<SorterEntry> entries)
    {
        foreach (var sorter in entries)
        {
            if (sorter is null)
                continue;

            var cached = _querySorterCache.Get(sorter);
            if (cached is not null)
            {
                yield return (QuerySorter<T>)cached;
                continue;
            }

            var keySelector = GetKeySelector(sorter);
            if (keySelector != null)
            {
                var querySorter = new QuerySorter<T>(keySelector, sorter.Descending);
                yield return querySorter;
                _querySorterCache.Set(sorter, querySorter);
            }
        }
    }

    private IEnumerable<QuerySorter<T>> BuildSortersFromKeys(IEnumerable<string> sortKeys)
    {
        var sorters = new List<SorterEntry>(sortKeys.Count());

        foreach (var sorterKey in sortKeys)
        {
            if (string.IsNullOrEmpty(sorterKey))
                continue;

            var isDescending = sorterKey.StartsWith("-");

            var sorter = new SorterEntry()
            {
                Property = isDescending ? sorterKey[1..] : sorterKey,
                Descending = isDescending
            };

            sorters.Add(sorter);
        }

        return BuildSortersFromEntries(sorters);
    }

    private Expression<Func<T, object>>? GetKeySelector(SorterEntry entry)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertySelector = GetPropertySelector(entry.Property, parameter);

            var converted = Expression.Convert(propertySelector, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, parameter);
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
}

public interface ISorterBuilder<T> where T : class
{
    IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase? request);
}
