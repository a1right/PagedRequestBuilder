using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    internal partial class PagedRequestValueParser : IPagedRequestValueParser
    {
        public ValueParseResult GetValue(JsonNode node, Type assignablePropertyType)
        {
            if (node is JsonArray array)
                return ParseArray(node, assignablePropertyType);

            if (node is JsonValue value)
                return GetStrategy(node).GetValue(node, assignablePropertyType);

            throw new NotImplementedException();
        }

        public IValueParseStrategy GetStrategy(JsonNode node)
        {
            var value = node.GetValue<JsonElement>();

            if (value.ValueKind is JsonValueKind.Number)
                return new NumberParseStrategy();

            if (value.ValueKind is JsonValueKind.String)
                return new StringParseStrategy();

            if (value.ValueKind is JsonValueKind.True or JsonValueKind.False)
                return new BoolParseStrategy();

            throw new NotImplementedException();
        }

        public ValueParseResult ParseArray(JsonNode node, Type assignablePropertyType)
        {
            var value = node.AsArray().ToArray();
            var parseResult = value.Select(x => GetValue(x, assignablePropertyType)).ToArray();
            var result = Array.CreateInstance(parseResult.First().ValueType, parseResult.Length);

            for (var index = 0; index < parseResult.Length; index++)
            {
                result.SetValue(parseResult[index].Value, index);
            }

            return new ValueParseResult(result, result.GetType());
        }
    }

    public interface IPagedRequestValueParser
    {
        ValueParseResult GetValue(JsonNode node, Type assignablePropertyType);
    }
}
