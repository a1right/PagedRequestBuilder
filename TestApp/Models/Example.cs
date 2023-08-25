using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Models;

public class Example
{
    [PagedRequestKey("id")]
    public int Id { get; set; }
    [PagedRequestKey("string")]
    public string String { get; set; }
    [PagedRequestKey("date")]
    public DateTime Date { get; set; }
    [PagedRequestKey("enum")]
    public ExampleEnum Enum { get; set; }
    [PagedRequestKey("decimal")]
    public decimal Decimal { get; set; }
    [PagedRequestKey("decimals")]
    public List<decimal> Decimals { get; set; }
    [PagedRequestKey("ints")]
    public int[] Ints { get; set; }
    [PagedRequestKey("guid")]
    public Guid Guid { get; set; }
    [PagedRequestKey("inner")]
    public InnerExample Inner { get; set; }
}

public class InnerExample
{
    public int Id { get; set; }
    [PagedRequestKey("string")]
    public string String { get; set; }
    [PagedRequestKey("doubleinner")]
    public DoubleInnerExample Nested { get; set; }
}

public class DoubleInnerExample
{
    public int Id { get; set; }
    [PagedRequestKey("string")]
    public string String { get; set; }
    [PagedRequestKey("enumlist")]
    public List<ExampleEnum> EnumList { get; set; }

    [PagedRequestKey("enumarray")]
    public ExampleEnum[] EnumArray { get; set; }
}
