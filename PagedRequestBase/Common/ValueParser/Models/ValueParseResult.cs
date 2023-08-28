using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser.Models;

public class ValueParseResult
{
    public object? Value { get; }
    public Type ValueType { get; }

    public ValueParseResult(object? value, Type valueType)
    {
        Value = value;
        ValueType = valueType;
    }

    public static ValueParseResult New<T>(T value) => new(value, typeof(T));
    public static ValueParseResult New<T>(T value, Type valueType) => new(value, valueType);
    public static ValueParseResult New<T>(JsonNode value) => new(value.GetValue<T>(), typeof(T));
}
