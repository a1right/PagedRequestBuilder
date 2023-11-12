using PagedRequestBuilder.Common;
using PagedRequestBuilder.Extensions;
using PagedRequestBuilder.Models;
using System.Linq;

namespace PagedRequestBuilder.Builders;

internal class PagedQueryBuilder<T> : IPagedQueryBuilder<T>
    where T : class
{
    private readonly IQueryStringParametersMapper _parametersMapper;
    private readonly IFilterBuilder<T> _filterBuilder;
    private readonly ISorterBuilder<T> _sorterBuilder;
    public PagedQueryBuilder(
        IFilterBuilder<T> filterBuilder,
        ISorterBuilder<T> sorterBuilder,
        IQueryStringParametersMapper parametersMapper)
    {
        _filterBuilder = filterBuilder;
        _sorterBuilder = sorterBuilder;
        _parametersMapper = parametersMapper;
    }

    public TQueryable BuildQuery<TQueryable>(TQueryable query, PagedRequestBase request) where TQueryable : IQueryable<T>
    {
        _parametersMapper.MapQueryStringParams(request);

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
