using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    internal partial class PagedRequestValueParser
    {
        public class BoolParseStrategy : IValueParseStrategy
        {
            public ValueParseResult GetValue(JsonNode node, Type assignablePropertyType)
            {
                var value = node.GetValue<JsonElement>();

                if (value.ValueKind is JsonValueKind.True)
                    return new ValueParseResult(true, typeof(bool));

                if (value.ValueKind is JsonValueKind.False)
                    return new ValueParseResult(false, typeof(bool));

                throw new NotImplementedException();
            }
        }
    }
}
