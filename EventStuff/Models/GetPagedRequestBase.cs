using System.Collections.Generic;

namespace EventStuff.Models
{
    public abstract class GetPagedRequestBase<T>
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

    public class PagedResponse<T>
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public int? Total { get; set; }
        public List<T> Data { get; set; }
        public PagedResponse(List<T> data, int? page, int? size, int? total)
        {
            Data = data;
            Page = page;
            Size = size;
            Total = total;
        }
    }

    public class GetPagedExampleRequest : GetPagedRequestBase<Example>
    {
    }
}
