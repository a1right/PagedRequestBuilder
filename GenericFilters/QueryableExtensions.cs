using System.Linq;

namespace GenericFilters
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> BuildQuery<T>(this IQueryable<T> queryable, string? request)
        {
            if (request is null)
                return queryable;

            var filterBuilder = new FilterBuilder();
            var filter = filterBuilder.BuildFilter(request);

            var queryBuilder = new QueryBuilder();
            return queryBuilder.BuildQuery(queryable, filter);
        }
    }
}
