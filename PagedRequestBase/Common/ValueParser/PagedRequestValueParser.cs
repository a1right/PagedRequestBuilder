using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    internal class PagedRequestValueParser : IPagedRequestValueParser
    {
        public ValueParseResult GetValue(JsonNode node, Type assignablePropertyType)
        {
            if (node is JsonArray array)
                return ArrayDeserealizationStrategy(node, assignablePropertyType);

            var value = node.GetValue<JsonElement>();

            if (node.)
                if (value.ValueKind is JsonValueKind.Number)
                    return NumberDeserealizationStrategy(node, assignablePropertyType);

            if (value.ValueKind is JsonValueKind.String)
                return StringDeserealizationStrategy(node);

            if (value.ValueKind is JsonValueKind.True)
                return new ValueParseResult(true, typeof(bool));

            if (value.ValueKind is JsonValueKind.False)
                return new ValueParseResult(false, typeof(bool));

            if (value.ValueKind is JsonValueKind.Array)
                return ArrayDeserealizationStrategy(node, assignablePropertyType);

            throw new NotImplementedException();
        }
        private ValueParseResult NumberDeserealizationStrategy(JsonNode node, Type assignablePropertyType)
        {
            if (assignablePropertyType.IsEnum)
            {
                var result = Enum.ToObject(assignablePropertyType, node.GetValue<byte>());
                return new ValueParseResult(result, result.GetType());
            }

            if (assignablePropertyType == typeof(int))
                return new ValueParseResult(node.GetValue<int>(), typeof(int));

            if (assignablePropertyType == typeof(double))
                return new ValueParseResult(node.GetValue<double>(), typeof(double));

            if (assignablePropertyType == typeof(decimal))
                return new ValueParseResult(node.GetValue<decimal>(), typeof(decimal));

            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType().IsEnum)
            {
                var result = Enum.ToObject(assignablePropertyType.GetElementType(), node.GetValue<byte>());
                return new ValueParseResult(result, result.GetType());
            }

            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(int))
                return new ValueParseResult(node.GetValue<int>(), typeof(int));
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(double))
                return new ValueParseResult(node.GetValue<double>(), typeof(double));
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(decimal))
                return new ValueParseResult(node.GetValue<decimal>(), typeof(decimal));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(int))
                return new ValueParseResult(node.GetValue<int>(), typeof(int));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(double))
                return new ValueParseResult(node.GetValue<double>(), typeof(double));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(decimal))
                return new ValueParseResult(node.GetValue<decimal>(), typeof(decimal));

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First().IsEnum)
            {
                var result = Enum.ToObject(assignablePropertyType.GetGenericArguments().First(), node.GetValue<byte>());
                return new ValueParseResult(result, result.GetType());
            }

            throw new NotImplementedException();
        }
        private ValueParseResult StringDeserealizationStrategy(JsonNode node)
        {
            if (DateTime.TryParse(node.GetValue<string>(), out var dateTime))
                return new ValueParseResult(dateTime.ToUniversalTime(), typeof(DateTime));

            if (Guid.TryParse(node.GetValue<string>(), out var guid))
                return new ValueParseResult(guid, typeof(Guid));

            return new ValueParseResult(node.GetValue<string>(), typeof(string));
        }

        private ValueParseResult ArrayDeserealizationStrategy(JsonNode node, Type assignablePropertyType)
        {
            var value = node.AsArray().ToArray();
            var parseResult = value.Select(x => GetValue(x, assignablePropertyType)).ToArray();
            var result = Array.CreateInstance(parseResult.First().ValueType, parseResult.Length);

            for (var index = 0; index < parseResult.Length; index++)
            {
                result.SetValue(parseResult[index].Value, index);
            }

            return new ValueParseResult(result, result.GetType());
        }
    }

    public interface IPagedRequestValueParser
    {
        ValueParseResult GetValue(JsonNode node, Type assignablePropertyType);
    }

    public class ValueParseResult
    {
        public object Value { get; set; }
        public Type ValueType { get; set; }

        public ValueParseResult(object value, Type valueType)
        {
            Value = value;
            ValueType = valueType;
        }
    }
}
