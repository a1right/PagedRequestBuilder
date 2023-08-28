using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using System;
using System.Linq;

namespace PagedRequestBuilder.Builders;

internal class PagedQueryBuilder<T> : IPagedQueryBuilder<T>
    where T : class
{
    private readonly IFilterBuilder<T> _filterBuilder;
    private readonly ISorterBuilder<T> _sorterBuilder;
    public PagedQueryBuilder(IFilterBuilder<T> filterBuilder, ISorterBuilder<T> sorterBuilder)
    {
        _filterBuilder = filterBuilder;
        _sorterBuilder = sorterBuilder;
    }

    public TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request) where TQueryable : IQueryable<T> => (TQueryable)BuildQueryBase(query, request);

    public TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate) where TQueryable : IQueryable<T> =>
        (TQueryable)paginate(BuildQueryBase(query, request));

    public TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request, int? page, int? size) where TQueryable : IQueryable<T> =>
        (TQueryable)BuildQueryBase(query, request)
        .Paginate(size, page);
    private IQueryable<T> BuildQueryBase(IQueryable<T> query, PagedRequestBase<T> request) => query
        .Where(_filterBuilder.BuildFilters(request))
        .OrderBy(_sorterBuilder.BuildSorters(request));
}

public interface IPagedQueryBuilder<T>
{
    TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request) where TQueryable : IQueryable<T>;
    TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request, Func<IQueryable<T>, IQueryable<T>> paginate) where TQueryable : IQueryable<T>;
    TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase<T> request, int? page, int? size) where TQueryable : IQueryable<T>;
}
