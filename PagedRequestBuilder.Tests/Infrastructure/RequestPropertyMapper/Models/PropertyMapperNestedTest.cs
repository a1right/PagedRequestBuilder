using PagedRequestBuilder.Attributes;

namespace PagedRequestBuilder.Tests.Infrastructure.RequestPropertyMapper.Models;

public class PropertyMapperNestedTest
{
    [PagedRequestKey(RequestPropertyMapperTests.Key)]
    public Guid Id { get; set; }
}
