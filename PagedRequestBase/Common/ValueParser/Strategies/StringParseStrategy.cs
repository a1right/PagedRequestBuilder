using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Collections;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class StringParseStrategy : IValueParseStrategy
{
    public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType)
    {
        if (assignablePropertyType.IsArray)
            return ForArrayAssignType(value, assignablePropertyType);

        if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType != typeof(string))
            return ForEnumerableAssignType(value, assignablePropertyType);

        return ForPrimitiveAssignType(value, assignablePropertyType);
    }

    private ValueParseResult ForPrimitiveAssignType(JsonValue value, Type assignablePropertyType)
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

    private ValueParseResult ForArrayAssignType(JsonValue value, Type assignablePropertyType)
    {
        var arrayOfType = assignablePropertyType.GetElementType();
        return ForPrimitiveAssignType(value, arrayOfType);
    }

    private ValueParseResult ForEnumerableAssignType(JsonValue value, Type assignablePropertyType)
    {
        var enumerableOfType = assignablePropertyType.GetGenericArguments().First();
        return ForPrimitiveAssignType(value, enumerableOfType);
    }
}
