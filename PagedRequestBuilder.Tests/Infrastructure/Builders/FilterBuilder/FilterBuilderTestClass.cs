using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Tests.Infrastructure.Builders.FilterBuilder;
public class FilterBuilderTestClass
{
    public const string IdPagedKey = "id";
    public const string IntPagedKey = "int";
    public const string StringPagedKey = "string";
    public const string GuidPagedKey = "guid";

    [PagedRequestKey(IdPagedKey)]
    public int Id { get; set; }
    [PagedRequestKey(StringPagedKey)]
    public string String { get; set; }
    [PagedRequestKey(IntPagedKey)]
    public int Int { get; set; }
    [PagedRequestKey(GuidPagedKey)]
    public Guid Guid { get; set; }
}