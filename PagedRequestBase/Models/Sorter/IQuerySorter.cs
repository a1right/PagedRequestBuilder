using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Sorter
{
    public interface IQuerySorter<T>
    {
        public bool Descending { get; }
        public Expression<Func<T, object>> Sorter { get; }
    }
}
