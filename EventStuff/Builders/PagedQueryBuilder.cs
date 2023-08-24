using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using System;
using System.Linq;

namespace PagedRequestBuilder.Builders
{
    public class PagedQueryBuilder<T> : IPagedQueryBuilder<T>
        where T : class
    {
        private readonly IFilterBuilder<T> _filterBuilder;
        private readonly ISorterBuilder<T> _sorterBuilder;
        public PagedQueryBuilder(IFilterBuilder<T> filterBuilder, ISorterBuilder<T> sorterBuilder)
        {
            _filterBuilder = filterBuilder;
            _sorterBuilder = sorterBuilder;
        }

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request) => BuildQueryBase(query, request);

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate) =>
            paginate(BuildQueryBase(query, request));

        public IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, bool paginate, int? page, int? size) =>
            BuildQueryBase(query, request)
            .Paginate(size, page);

        private IQueryable<T> BuildQueryBase(IQueryable<T> query, PagedRequestBase<T> request)
        {
            query = query
                .Where(_filterBuilder.BuildFilters(request))
                .OrderBy(_sorterBuilder.BuildSorters(request));

            return query;
        }
    }

    public interface IPagedQueryBuilder<T>
    {
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request);
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate);
        IQueryable<T> BuildQuery(IQueryable<T> query, PagedRequestBase<T> request, bool paginate, int? page, int? size);
    }
}
