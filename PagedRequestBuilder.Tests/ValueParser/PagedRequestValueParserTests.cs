using PagedRequestBuilder.Common.ValueParser;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Tests.ValueParser;

public class PagedRequestValueParserTests
{
    private PagedRequestValueParser _parser;
    public PagedRequestValueParserTests()
    {
        _parser = new PagedRequestValueParser(new ValueParseStrategyProvider(new(), new(), new()));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]

    public void GetValue_IntNode_ReturnsIntValue(int depth)
    {
        //arrange

        var data = CreateJaggedArray<int[][][]>(depth, depth, depth);
        var node = JsonNode.Parse(JsonSerializer.Serialize(data));

        //act
        var result = _parser.GetValue(node, typeof(int));
        var expected = JsonSerializer.Serialize(data);
        var actual = JsonSerializer.Serialize(result.Value);

        //assert
        Assert.NotNull(result);
        Assert.Equal(expected, actual);
    }

    T CreateJaggedArray<T>(params int[] lengths)
    {
        return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
    }

    object InitializeJaggedArray(Type type, int index, int[] lengths)
    {
        var array = Array.CreateInstance(type, lengths[index]);
        var elementType = type.GetElementType();

        if (elementType != null)
        {
            for (var i = 0; i < lengths[index]; i++)
            {
                array.SetValue(
                    InitializeJaggedArray(elementType, index + 1, lengths), i);
            }
        }

        return array;
    }
}
