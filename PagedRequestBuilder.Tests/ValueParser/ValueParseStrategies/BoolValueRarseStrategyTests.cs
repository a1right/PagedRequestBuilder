using PagedRequestBuilder.Common.ValueParser.Strategies;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.ValueParser.Strategies;

public class BoolValueRarseStrategyTests
{
    private readonly BoolParseStrategy _strategy;
    public BoolValueRarseStrategyTests()
    {
        _strategy = new BoolParseStrategy();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetValue_FromInteger_ReturnsIntegerValue(bool value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;
        var typeOfValue = value.GetType();

        //Act
        var actual = _strategy.Parse(jsonValue, typeOfValue);

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeOfValue, actual.ValueType);
    }
}