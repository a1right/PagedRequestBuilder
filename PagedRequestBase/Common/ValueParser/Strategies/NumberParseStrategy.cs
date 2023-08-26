using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Collections;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class NumberParseStrategy : IValueParseStrategy
{
    public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType)
    {
        if (assignablePropertyType.IsArray)
            return ForArrayAssignType(value, assignablePropertyType);

        if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType))
            return ForEnumerableAssignType(value, assignablePropertyType);

        return ForPrimitiveAssignType(value, assignablePropertyType);
    }

    public ValueParseResult ForPrimitiveAssignType(JsonValue value, Type assignablePropertyType)
    {
        if (assignablePropertyType.IsEnum)
            return ValueParseResult.New(Enum.ToObject(assignablePropertyType, value.GetValue<byte>()), assignablePropertyType);

        if (assignablePropertyType == typeof(int))
            return ValueParseResult.New<int>(value);

        if (assignablePropertyType == typeof(double))
            return ValueParseResult.New<double>(value);

        if (assignablePropertyType == typeof(decimal))
            return ValueParseResult.New<decimal>(value);

        throw new NotImplementedException();
    }

    public ValueParseResult ForArrayAssignType(JsonValue value, Type assignablePropertyType)
    {
        var arrayOfType = assignablePropertyType.GetElementType();

        if (arrayOfType.IsEnum)
            return ValueParseResult.New(Enum.ToObject(arrayOfType, value.GetValue<byte>()), arrayOfType);

        if (arrayOfType == typeof(int))
            return ValueParseResult.New<int>(value);

        if (arrayOfType == typeof(double))
            return ValueParseResult.New<double>(value);

        if (arrayOfType == typeof(decimal))
            return ValueParseResult.New<decimal>(value);

        throw new NotImplementedException();
    }

    public ValueParseResult ForEnumerableAssignType(JsonValue value, Type assignablePropertyType)
    {
        var enumerableOfType = assignablePropertyType.GetGenericArguments().First();

        if (enumerableOfType.IsEnum)
            return ValueParseResult.New(Enum.ToObject(enumerableOfType, value.GetValue<byte>()), enumerableOfType);

        if (enumerableOfType == typeof(int))
            return ValueParseResult.New<int>(value);

        if (enumerableOfType == typeof(double))
            return ValueParseResult.New<double>(value);

        if (enumerableOfType == typeof(decimal))
            return ValueParseResult.New<decimal>(value);

        throw new NotImplementedException();
    }
}
