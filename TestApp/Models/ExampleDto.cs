using System.Text.Json;

namespace PagedRequestBuilder.Models;

public class ExampleDto
{
    public string? String { get; set; }
    public string[]? Strings { get; set; }
    public DateTime Date { get; set; }
    public ExampleEnum Enum { get; set; }
    public decimal Decimal { get; set; }
    public Guid Guid { get; set; }
    public List<decimal>? Decimals { get; set; }
    public int[]? Ints { get; set; }
    public InnerExample? Inner { get; set; }
}

public static class ExampleMappings
{
    public static TOut Map<TIn, TOut>(this TIn src)
        where TIn : class
        where TOut : class
    {
        var serialized = JsonSerializer.Serialize(src);
        return JsonSerializer.Deserialize<TOut>(serialized)!; // :P
    }
}