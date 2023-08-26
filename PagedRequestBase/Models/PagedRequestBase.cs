using PagedRequestBuilder.Models.Filter;
using PagedRequestBuilder.Models.Sorter;
using System.Collections.Generic;

namespace PagedRequestBuilder.Models;

public abstract class PagedRequestBase<T>
{
    private int? _size;
    private int? _page;

    public List<FilterEntry>? Filters { get; set; } = new();
    public List<SorterEntry>? Sorters { get; set; } = new();
    public int? Page
    {
        get => _page ?? 1;
        set => _page = value;
    }
    public int? Size
    {
        get => _size ?? 10;
        set => _size = value;
    }
}
