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
        {
            var result = Enum.ToObject(assignablePropertyType, value.GetValue<byte>());
            return new ValueParseResult(result, result.GetType());
        }

        if (assignablePropertyType == typeof(int))
            return new ValueParseResult(value.GetValue<int>(), typeof(int));

        if (assignablePropertyType == typeof(double))
            return new ValueParseResult(value.GetValue<double>(), typeof(double));

        if (assignablePropertyType == typeof(decimal))
            return new ValueParseResult(value.GetValue<decimal>(), typeof(decimal));

        throw new NotImplementedException();
    }

    public ValueParseResult ForArrayAssignType(JsonValue value, Type assignablePropertyType)
    {
        var arrayOfType = assignablePropertyType.GetElementType();

        if (arrayOfType.IsEnum)
        {
            var result = Enum.ToObject(assignablePropertyType.GetElementType(), value.GetValue<byte>());
            return new ValueParseResult(result, result.GetType());
        }

        if (arrayOfType == typeof(int))
            return new ValueParseResult(value.GetValue<int>(), typeof(int));

        if (arrayOfType == typeof(double))
            return new ValueParseResult(value.GetValue<double>(), typeof(double));

        if (arrayOfType == typeof(decimal))
            return new ValueParseResult(value.GetValue<decimal>(), typeof(decimal));

        throw new NotImplementedException();
    }

    public ValueParseResult ForEnumerableAssignType(JsonValue value, Type assignablePropertyType)
    {
        var enumerableOfType = assignablePropertyType.GetGenericArguments().First();

        if (enumerableOfType.IsEnum)
        {
            var result = Enum.ToObject(assignablePropertyType.GetGenericArguments().First(), value.GetValue<byte>());
            return new ValueParseResult(result, result.GetType());
        }

        if (enumerableOfType == typeof(int))
            return new ValueParseResult(value.GetValue<int>(), typeof(int));

        if (enumerableOfType == typeof(double))
            return new ValueParseResult(value.GetValue<double>(), typeof(double));

        if (enumerableOfType == typeof(decimal))
            return new ValueParseResult(value.GetValue<decimal>(), typeof(decimal));

        throw new NotImplementedException();
    }
}
