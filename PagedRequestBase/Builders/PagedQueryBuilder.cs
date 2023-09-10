using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
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

    public TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase request) where TQueryable : IQueryable<T>
    {
        var result = query
            .Where(_filterBuilder.BuildFilters(request))
            .OrderBy(_sorterBuilder.BuildSorters(request));

        return (TQueryable)result;
    }
}

public interface IPagedQueryBuilder<T>
{
    TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase request) where TQueryable : IQueryable<T>;
}
