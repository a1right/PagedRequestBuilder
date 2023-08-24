using System;
using System.Linq.Expressions;

namespace PagedRequestBuilder.Models.Filter
{
    public interface IQueryFilter<T>
    {
        public Expression<Func<T, bool>> Filter { get; }
    }
}
