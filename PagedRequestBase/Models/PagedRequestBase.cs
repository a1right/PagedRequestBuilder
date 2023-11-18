using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Models.Sorter;
using System;

namespace PagedRequestBuilder.Models;
public struct PagedRequestBase
{
    private int? _size;
    private int? _page;
    public PagedRequestBase()
    {

    }
    public PagedRequestBase(FilterEntry[] filters, FilterEntry[][] complexFilters, SorterEntry[] sorters, string[] sortKeys, string? query, int? page, int? size)
    {
        Filters = filters;
        ComplexFilters = complexFilters;
        Sorters = sorters;
        SortKeys = sortKeys;
        _page = page;
        _size = size;
        Query = query;
    }

    public FilterEntry[] Filters { get; set; } = Array.Empty<FilterEntry>();
    public FilterEntry[][] ComplexFilters { get; set; } = Array.Empty<FilterEntry[]>();
    public SorterEntry[] Sorters { get; set; } = Array.Empty<SorterEntry>();
    public string[] SortKeys { get; set; } = Array.Empty<string>();
    public string? Query { get; set; }
    public int Page
    {
        get => _page ?? PaginationSetting.DefaultPageNumber;
        set => _page = value;
    }
    public int Size
    {
        get => _size ?? PaginationSetting.DefaultPageSize;
        set => _size = value;
    }
}