using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Collections;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Strategies;

internal class NumberParseStrategy : IValueParseStrategy
{
    public ValueParseResult Parse(JsonValue value, Type assignablePropertyType)
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

        if (Nullable.GetUnderlyingType(assignablePropertyType).IsEnum)
            return ValueParseResult.New(Enum.ToObject(Nullable.GetUnderlyingType(assignablePropertyType), value.GetValue<byte>()), Nullable.GetUnderlyingType(assignablePropertyType));

        if (assignablePropertyType == typeof(int?))
            return ValueParseResult.New<int?>(value);

        if (assignablePropertyType == typeof(double?))
            return ValueParseResult.New<double?>(value);

        if (assignablePropertyType == typeof(decimal?))
            return ValueParseResult.New<decimal?>(value);

        throw new NotImplementedException();
    }

    public ValueParseResult ForArrayAssignType(JsonValue value, Type assignablePropertyType)
    {
        var arrayOfType = assignablePropertyType.GetElementType();

        return ForPrimitiveAssignType(value, arrayOfType);
    }

    public ValueParseResult ForEnumerableAssignType(JsonValue value, Type assignablePropertyType)
    {
        var enumerableOfType = assignablePropertyType.GetGenericArguments().First();

        return ForPrimitiveAssignType(value, enumerableOfType);
    }
}
