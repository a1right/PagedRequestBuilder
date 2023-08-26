using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    public interface IValueParseStrategy
    {
        ValueParseResult GetValue(JsonNode node, Type assignablePropertyType);
    }
}
