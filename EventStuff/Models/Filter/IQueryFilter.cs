using System;
using System.Linq.Expressions;

namespace EventStuff.Models.Filter
{
    public interface IQueryFilter<T>
    {
        public Expression<Func<T, bool>> Filter { get; }
    }
}
