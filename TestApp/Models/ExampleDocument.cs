using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Models;

public class ExampleDocument
{
    [PagedRequestKey("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [PagedRequestKey("string")]
    public string? String { get; set; }
    [PagedRequestKey("strings")]
    public List<string>? Strings { get; set; }
    [PagedRequestKey("date")]
    public DateTime Date { get; set; }
    [PagedRequestKey("enum")]
    public ExampleEnum Enum { get; set; }
    [PagedRequestKey("decimal")]
    public decimal Decimal { get; set; }
    [PagedRequestKey("decimals")]
    public List<decimal>? Decimals { get; set; }
    [PagedRequestKey("ints")]
    public int[]? Ints { get; set; }
    [PagedRequestKey("guid")]
    public Guid Guid { get; set; }
}
