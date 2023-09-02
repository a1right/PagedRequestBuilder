using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Tests.Infrastructure.RequestPropertyMapper.Models;

namespace PagedRequestBuilder.Tests;

public class RequestPropertyMapperTests
{
    public const string Key = "key exists";
    public const string NestedKey = "nested key exists";
    public const string KeyNotExists = "key not exists";

    private RequestPropertyMapper _mapper;
    public RequestPropertyMapperTests()
    {
        PagedQueryBuilder.Initialize();
        _mapper = new RequestPropertyMapper();
    }
    [Fact]
    public void MapRequestNameToPropertyName_PropertyKeyExists_ReturnPropertyName()
    {
        var actual = _mapper.MapRequestNameToPropertyName<PropertyMapperTest>(Key);
        var expected = nameof(PropertyMapperTest.Id);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MapRequestNameToPropertyName_PropertyKeyNotExists_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _mapper.MapRequestNameToPropertyName<PropertyMapperTest>(KeyNotExists));
    }

    [Fact]
    public void MapNestedRequestNameToPropertyName_NestedPropertyKeyExists_ReturnPropertyName()
    {
        var actual = _mapper.MapNestedRequestNameToPropertyName<PropertyMapperTest>(Key, typeof(PropertyMapperNestedTest));
        var expected = nameof(PropertyMapperNestedTest.Id);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MapNestedRequestNameToPropertyName_NestedPropertyKeyNotExists_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _mapper.MapNestedRequestNameToPropertyName<PropertyMapperTest>(KeyNotExists, typeof(PropertyMapperNestedTest)));
    }
}
