using PagedRequestBuilder.Common.ValueParser.Strategies;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser;

internal class ValueParseStrategyProvider : IValueParseStrategyProvider
{
    private readonly StringParseStrategy _stringParseStrategy;
    private readonly NumberParseStrategy _numberParseStrategy;
    private readonly BoolParseStrategy _boolParseStrategy;

    public ValueParseStrategyProvider(
        StringParseStrategy stringParseStrategy,
        NumberParseStrategy numberParseStrategy,
        BoolParseStrategy boolParseStrategy)
    {
        _stringParseStrategy = stringParseStrategy;
        _numberParseStrategy = numberParseStrategy;
        _boolParseStrategy = boolParseStrategy;
    }

    public IValueParseStrategy ForNode(JsonNode node)
    {
        var value = node.GetValue<JsonElement>();

        if (value.ValueKind is JsonValueKind.Number)
            return _numberParseStrategy;

        if (value.ValueKind is JsonValueKind.String)
            return _stringParseStrategy;

        if (value.ValueKind is JsonValueKind.True or JsonValueKind.False)
            return _boolParseStrategy;

        throw new NotImplementedException();
    }

}

internal interface IValueParseStrategyProvider
{
    IValueParseStrategy ForNode(JsonNode node);
}