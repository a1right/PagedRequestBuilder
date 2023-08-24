using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Models;

public class Example
{
    [PagedRequestKeyAttribute("id")]
    public int Id { get; set; }
    [PagedRequestKeyAttribute("string")]
    public string String { get; set; }
    [PagedRequestKeyAttribute("date")]
    public DateTime Date { get; set; }
    [PagedRequestKeyAttribute("enum")]
    public ExampleEnum Enum { get; set; }
    [PagedRequestKeyAttribute("decimal")]
    public decimal Decimal { get; set; }
    [PagedRequestKey("decimals")]
    public List<decimal> Decimals { get; set; }
    [PagedRequestKey("ints")]
    public int[] Ints { get; set; }
    [PagedRequestKeyAttribute("guid")]
    public Guid Guid { get; set; }
}
