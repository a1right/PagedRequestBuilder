using System.Linq;

namespace GenericFilters
{
    public interface IQueryBuilder
    {
        IQueryable<T> BuildQuery<T>(IQueryable<T> query, Filter requestFilters);
    }
}
