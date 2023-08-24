using System.Collections.Generic;

namespace PagedRequestBuilder.Models
{
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
}
