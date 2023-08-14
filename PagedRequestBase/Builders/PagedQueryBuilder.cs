using EventStuff.Extensions;
using EventStuff.Models;
using System;
using System.Linq;

namespace EventStuff.Builders
{
    internal class PagedQueryBuilder<T> : IPagedQueryBuilder<T>
        where T : class
    {
        private readonly IFilterBuilder<T> _filterBuilder;
        private readonly ISorterBuilder<T> _sorterBuilder;
        internal PagedQueryBuilder(IFilterBuilder<T> filterBuilder, ISorterBuilder<T> sorterBuilder)
        {
            _filterBuilder = filterBuilder;
            _sorterBuilder = sorterBuilder;
        }

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request)
        {
            var filters = _filterBuilder.BuildFilters(request);
            var sorters = _sorterBuilder.BuildSorters(request);

            return query
                .Where(filters)
                .OrderBy(sorters);
        }

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate)
        {
            var filters = _filterBuilder.BuildFilters(request);
            var sorters = _sorterBuilder.BuildSorters(request);

            query = paginate(query);

            return query
                .Where(filters)
                .OrderBy(sorters);
        }

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, bool paginate, int? page, int? size)
        {
            var filters = _filterBuilder.BuildFilters(request);
            var sorters = _sorterBuilder.BuildSorters(request);

            if (paginate)
                query = query.Paginate(size, page);

            return query
                .Where(filters)
                .OrderBy(sorters);
        }
    }

    public interface IPagedQueryBuilder<T>
    {
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request);
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate);
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, bool paginate, int? page, int? size);
    }
}
