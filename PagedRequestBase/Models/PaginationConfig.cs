namespace PagedRequestBuilder.Models;
public class PaginationConfig
{
    public int DefaultPageNumber { get; set; }
    public int DefaultPageSize { get; set; }
    public bool ThrowExceptions { get; set; }
}
