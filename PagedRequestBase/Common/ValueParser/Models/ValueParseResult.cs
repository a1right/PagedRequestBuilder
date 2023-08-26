using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Models;

public class ValueParseResult
{
    public object? Value { get; private set; }
    public Type ValueType { get; private set; }

    public ValueParseResult(object value, Type valueType)
    {
        Value = value;
        ValueType = valueType;
    }

    public static ValueParseResult New<T>(T value) => new ValueParseResult(value, typeof(T));
    public static ValueParseResult New<T>(T value, Type valueType) => new ValueParseResult(value, valueType);
    public static ValueParseResult New<T>(JsonNode value) => new ValueParseResult(value.GetValue<T>(), typeof(T));
}
