using System;
using System.Text.Json;

namespace EventStuff.Builders
{
    internal class PagedRequestValueParser : IPagedRequestValueParser
    {
        public object? GetValue(JsonDocument document, Type assignablePropertyType)
        {
            var value = document.RootElement;
            if (value.ValueKind is JsonValueKind.Number)
                return NumberDeserealizationStrategy(value, assignablePropertyType);

            if (value.ValueKind is JsonValueKind.String)
                return StringDeserealizationStrategy(value);

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
        private object NumberDeserealizationStrategy(JsonElement document, Type assignablePropertyType)
        {
            var value = document.Deserialize<decimal>().ToString();

            if (value is null)
                throw new NotImplementedException();

            if (assignablePropertyType.IsEnum)
                return Enum.ToObject(assignablePropertyType, byte.Parse(value));

            if (assignablePropertyType == typeof(int))
                return int.Parse(value);

            if (assignablePropertyType == typeof(double))
                return double.Parse(value);

            if (assignablePropertyType == typeof(decimal))
                return decimal.Parse(value);

            throw new NotImplementedException();
        }
        private object? StringDeserealizationStrategy(JsonElement element)
        {
            var value = element.Deserialize<string>();

            if (DateTime.TryParse(value, out var dateTime))
                return dateTime.ToUniversalTime();

            if (Guid.TryParse(value, out var guid))
                return guid;

            return value;
        }

        private object[]? ArrayDeserealizationStrategy(JsonElement element)
        {
            var value = element.Deserialize<object[]>();

            return value;
        }
    }

    internal interface IPagedRequestValueParser
    {
        object? GetValue(JsonDocument document, Type assignablePropertyType);
    }
}
