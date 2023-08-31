using PagedRequestBuilder.Common.ValueParser.Strategies;
using PagedRequestBuilder.Tests.Infrastructure.ValueParser;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.ValueParser.Strategies;

public class NumberValueRarseStrategyTests
{
    private readonly NumberParseStrategy _strategy;
    public NumberValueRarseStrategyTests()
    {
        _strategy = new NumberParseStrategy();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void GetValue_FromInteger_ReturnsIntegerValue(int value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;
        var typeOfValue = value.GetType();

        //Act
        var actual = _strategy.GetValue(jsonValue, typeOfValue);

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeOfValue, actual.ValueType);
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(42.42d)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    public void GetValue_FromDouble_ReturnsDoubleValue(double value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;
        var typeOfValue = value.GetType();

        //Act
        var actual = _strategy.GetValue(jsonValue, typeOfValue);

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeOfValue, actual.ValueType);
    }

    [Theory]
    [InlineData("42.42")]
    [InlineData("0")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    public void GetValue_FromDecimal_ReturnsDecimalValue(string stringValue)
    {
        //Arrange
        var value = decimal.Parse(stringValue);
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;
        var typeOfValue = value.GetType();

        //Act
        var actual = _strategy.GetValue(jsonValue, typeOfValue);

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeOfValue, actual.ValueType);
    }

    [Theory]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData((TestEnum)4)]
    public void GetValue_FromEnum_ReturnsEnumValue(TestEnum value)
    {
        //Arrange
        var node = JsonNode.Parse(JsonSerializer.Serialize(value));
        var jsonValue = (JsonValue)node;
        var typeOfValue = value.GetType();

        //Act
        var actual = _strategy.GetValue(jsonValue, typeOfValue);

        //Assert
        Assert.Equal(value, actual.Value);
        Assert.Equal(typeOfValue, actual.ValueType);
    }
}
