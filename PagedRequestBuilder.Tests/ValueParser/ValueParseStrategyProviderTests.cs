using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Common.ValueParser.Strategies;
using PagedRequestBuilder.Tests.Infrastructure.ValueParser;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.ValueParser;

public class ValueParseStrategyProviderTests
{
    private readonly ValueParseStrategyProvider _provider;

    public ValueParseStrategyProviderTests()
    {
        _provider = new ValueParseStrategyProvider(new(), new(), new());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0d)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    [InlineData(TestEnum.One)]
    [InlineData((TestEnum)4)]
    public void ForNode_Number_ReturnsNumberParseStrategy(object value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));

        //Act
        var strategy = _provider.ForNode(node);

        //Assert
        Assert.True(strategy is NumberParseStrategy);
    }

    [Theory]
    [InlineData("42.42")]
    [InlineData("0")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    public void ForNode_DecimalNumber_ReturnsNumberParseStrategy(string stringValue)
    {
        //Arrange
        var value = decimal.Parse(stringValue);
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));

        //Act
        var strategy = _provider.ForNode(node);

        //Assert
        Assert.True(strategy is NumberParseStrategy);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ForNode_Bool_ReturnsBoolParseStrategy(bool value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));

        //Act
        var strategy = _provider.ForNode(node);

        //Assert
        Assert.True(strategy is BoolParseStrategy);
    }

    [Theory]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f1")]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f2")]
    [InlineData("ca0ea80a-322c-436d-8e23-c638a30cf8f3")]
    [InlineData("2023-08-31T06:56:55.382873Z")]
    [InlineData("2023-08-18T00:00:00Z")]
    [InlineData("2023-08-18T00:00:00+03:00")]
    [InlineData("test1")]
    [InlineData("another_test")]
    [InlineData("")]
    [InlineData("97ADFEAF-73D1-4DF6-ABD4-6DEDA3B3C3ED")]
    public void ForNode_String_ReturnsStringParseStrategy(string value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));

        //Act
        var strategy = _provider.ForNode(node);

        //Assert
        Assert.True(strategy is StringParseStrategy);
    }
}