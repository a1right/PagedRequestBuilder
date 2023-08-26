using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Collections;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    public class NumberParseStrategy : IValueParseStrategy
    {
        public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType)
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

            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType().IsEnum)
            {
                var result = Enum.ToObject(assignablePropertyType.GetElementType(), value.GetValue<byte>());
                return new ValueParseResult(result, result.GetType());
            }

            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(int))
                return new ValueParseResult(value.GetValue<int>(), typeof(int));
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(double))
                return new ValueParseResult(value.GetValue<double>(), typeof(double));
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(decimal))
                return new ValueParseResult(value.GetValue<decimal>(), typeof(decimal));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(int))
                return new ValueParseResult(value.GetValue<int>(), typeof(int));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(double))
                return new ValueParseResult(value.GetValue<double>(), typeof(double));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(decimal))
                return new ValueParseResult(value.GetValue<decimal>(), typeof(decimal));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First().IsEnum)
            {
                var result = Enum.ToObject(assignablePropertyType.GetGenericArguments().First(), value.GetValue<byte>());
                return new ValueParseResult(result, result.GetType());
            }

            throw new NotImplementedException();
        }
    }
}
