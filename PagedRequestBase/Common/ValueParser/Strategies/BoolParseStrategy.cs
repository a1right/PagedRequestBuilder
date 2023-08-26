using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class BoolParseStrategy : IValueParseStrategy
{
    public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType) => ValueParseResult.New<bool>(value);
}
