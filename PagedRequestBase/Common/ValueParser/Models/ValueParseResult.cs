using System;

namespace PagedRequestBuilder.Common.ValueParser.Models;

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
