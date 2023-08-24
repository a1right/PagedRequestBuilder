using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Builders
{
    public class PagedRequestValueParser : IPagedRequestValueParser
    {
        public object? GetValue(JsonNode node, Type assignablePropertyType)
        {
            var value = node.GetValue<JsonElement>();
            if (value.ValueKind is JsonValueKind.Number)
                return NumberDeserealizationStrategy(node, assignablePropertyType);

            if (value.ValueKind is JsonValueKind.String)
                return StringDeserealizationStrategy(node);

            if (value.ValueKind is JsonValueKind.True)
                return true;

            if (value.ValueKind is JsonValueKind.False)
                return false;

            if (value.ValueKind is JsonValueKind.Object)
                return value.Deserialize<object>();

            if (value.ValueKind is JsonValueKind.Array)
                return ArrayDeserealizationStrategy(value);

            throw new NotImplementedException();
        }
        private object NumberDeserealizationStrategy(JsonNode node, Type assignablePropertyType)
        {
            if (assignablePropertyType.IsEnum)
                return Enum.ToObject(assignablePropertyType, node.GetValue<byte>());

            if (assignablePropertyType == typeof(int))
                return node.GetValue<int>();

            if (assignablePropertyType == typeof(double))
                return node.GetValue<double>();

            if (assignablePropertyType == typeof(decimal))
                return node.GetValue<decimal>();

            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType().IsEnum)
                return Enum.ToObject(assignablePropertyType.GetElementType(), node.GetValue<byte>());
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(int))
                return node.GetValue<int>();
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(double))
                return node.GetValue<double>();
            if (assignablePropertyType.IsArray && assignablePropertyType.GetElementType() == typeof(decimal))
                return node.GetValue<decimal>();

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(int))
                return node.GetValue<int>();

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(double))
                return node.GetValue<double>();

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First() == typeof(decimal))
                return node.GetValue<decimal>();

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType) && assignablePropertyType.GetGenericArguments().First().IsEnum)
                return Enum.ToObject(assignablePropertyType.GetGenericArguments().First(), node.GetValue<byte>());

            throw new NotImplementedException();
        }
        private object? StringDeserealizationStrategy(JsonNode node)
        {
            if (DateTime.TryParse(node.GetValue<string>(), out var dateTime))
                return dateTime.ToUniversalTime();

            if (Guid.TryParse(node.GetValue<string>(), out var guid))
                return guid;

            return node.GetValue<string>();
        }

        private object[]? ArrayDeserealizationStrategy(JsonElement element)
        {
            var value = element.Deserialize<object[]>();

            return value;
        }
    }

    public interface IPagedRequestValueParser
    {
        object? GetValue(JsonNode node, Type assignablePropertyType);
    }
}
