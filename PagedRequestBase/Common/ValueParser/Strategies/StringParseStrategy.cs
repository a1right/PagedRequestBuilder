using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class StringParseStrategy : IValueParseStrategy
{
    public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType)
    {
        var typedValue = value.GetValue<string>();
        if (assignablePropertyType == typeof(DateTime))
            return ValueParseResult.New(DateTime.Parse(typedValue).ToUniversalTime());

        if (assignablePropertyType == typeof(Guid))
            return ValueParseResult.New(Guid.Parse(typedValue));

        if (assignablePropertyType == typeof(string))
            return ValueParseResult.New(typedValue);

        throw new NotImplementedException();
    }
}
