using PagedRequestBuilder.Common;
using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Models.Sorter;
using System.Collections.Generic;

namespace PagedRequestBuilder.Models;

public abstract class PagedRequestBase
{
    private int? _size;
    private int? _page;

    public List<FilterEntry> Filters { get; set; } = new();
    public List<List<FilterEntry>> ComplexFilters { get; set; } = new();
    public List<SorterEntry> Sorters { get; set; } = new();
    public List<string> SortKeys { get; set; } = new();
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

internal class PagedRequestModel : PagedRequestBase
{

}