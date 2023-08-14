using System;
using System.Linq.Expressions;

namespace EventStuff.Models.Sorter
{
    public interface IQuerySorter<T>
    {
        public bool Descending { get; }
        public Expression<Func<T, object>> Sorter { get; }
    }
}
