using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class BoolParseStrategy : IValueParseStrategy
{
    public ValueParseResult Parse(JsonValue value, Type assignablePropertyType) => ValueParseResult.New<bool>(value);
}
