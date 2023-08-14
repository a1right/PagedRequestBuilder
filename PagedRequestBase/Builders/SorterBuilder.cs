using EventStuff.Models;
using EventStuff.Models.Sorter;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EventStuff.Builders
{
    internal class SorterBuilder<T> : ISorterBuilder<T> where T : class
    {
        private readonly IPagedRequestPropertyMapper _propertyMapper;

        internal SorterBuilder(IPagedRequestPropertyMapper propertyMapper)
        {
            _propertyMapper = propertyMapper;
        }

        public IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase<T>? request)
        {
            if (request is null or { Sorters: not { Count: > 0 } })
                yield break;

            foreach (var sorter in request.Sorters)
            {
                var keySelector = GetKeySelector(sorter);

                if (keySelector != null)
                    yield return new QuerySorter<T>(keySelector, sorter.Descending ?? false);

            }
        }

        private Expression<Func<T, object>>? GetKeySelector(SorterEntry entry)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var typePropertyName = _propertyMapper.MapRequestNameToPropertyName<T>(entry.Property);
                var propertySelector = Expression.PropertyOrField(parameter, typePropertyName);
                var converted = Expression.Convert(propertySelector, typeof(object));
                return Expression.Lambda<Func<T, object>>(converted, parameter);
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }

    internal interface ISorterBuilder<T> where T : class
    {
        IEnumerable<IQuerySorter<T>> BuildSorters(PagedRequestBase<T>? request);
    }
}
