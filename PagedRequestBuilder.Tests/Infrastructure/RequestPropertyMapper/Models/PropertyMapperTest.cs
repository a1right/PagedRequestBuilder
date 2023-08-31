using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Tests.Infrastructure.RequestPropertyMapper.Models;

public class PropertyMapperTest
{
    [PagedRequestKey(RequestPropertyMapperTests.Key)]
    public int Id { get; set; }
    [PagedRequestKey(RequestPropertyMapperTests.NestedKey)]
    public PropertyMapperNestedTest? Nested { get; set; }
}
