using PagedRequestBuilder.Common.ValueParser.Strategies;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.ValueParser.Strategies;
public class StringValueParseStrategyTests
{
    private readonly StringParseStrategy _strategy;

    public StringValueParseStrategyTests()
    {
        _strategy = new StringParseStrategy();
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("another_test")]
    [InlineData("")]
    [InlineData("97ADFEAF-73D1-4DF6-ABD4-6DEDA3B3C3ED")]
    public void GetValue_FromString_ReturnsStringValue(string value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;

        //Act
        var actual = _strategy.Parse(jsonValue, typeof(string));

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeof(string), actual.ValueType);
    }

    [Theory]
    [InlineData("2023-08-31T06:56:55.382873Z")]
    [InlineData("2023-08-18T00:00:00Z")]
    [InlineData("2023-08-18T00:00:00+03:00")]
    public void GetValue_FromDateTimeString_ReturnsDateTimeValue(string value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;

        //Act
        var actual = _strategy.Parse(jsonValue, typeof(DateTime));
        var expected = DateTime.Parse(value).ToUniversalTime();

        //Assert
        Assert.Equal(expected, actual.Value);
        Assert.Equal(typeof(DateTime), actual.ValueType);
    }

    [Theory]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f1")]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f2")]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f3")]
    public void GetValue_FromGuidString_ReturnsGuidValue(string value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;

        //Act
        var actual = _strategy.Parse(jsonValue, typeof(Guid));
        var expected = Guid.Parse(value);

        //Assert
        Assert.Equal(expected, actual.Value);
        Assert.Equal(typeof(Guid), actual.ValueType);
    }
}
