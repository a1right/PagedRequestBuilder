using System;
using System.Linq.Expressions;
using System.Text.Json;

namespace EventStuff.Models
{
    public class FilterEntry
    {
        public string Property { get; set; }
        public JsonDocument Value { get; set; }
        public string Operation { get; set; }
    }

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

    public interface IQueryFilter<T>
    {
        public Expression<Func<T, bool>> Filter { get; }
    }

    public class SorterEntry
    {
        public string? Property { get; set; }
        public bool? Descending { get; set; }
    }

    public class QuerySorter<T> : IQuerySorter<T>
    {
        private Expression<Func<T, object>> _keySelector;
        private readonly bool _descending;

        public bool Descending => _descending;
        public Expression<Func<T, object>> Sorter => _keySelector;

        public QuerySorter(Expression<Func<T, object>> keySelector, bool descending = false)
        {
            _keySelector = keySelector;
            _descending = descending;
        }
        public static implicit operator Expression<Func<T, object>>(QuerySorter<T> filter) => filter._keySelector;
    }

    public interface IQuerySorter<T>
    {
        public bool Descending { get; }
        public Expression<Func<T, object>> Sorter { get; }
    }
}
