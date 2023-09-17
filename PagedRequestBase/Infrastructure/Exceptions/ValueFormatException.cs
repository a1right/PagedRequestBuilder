using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Infrastructure.Exceptions;

internal class ValueFormatException : PagedRequestBuilderException
{
    public ValueFormatException(JsonValue value, Type assignablePropertyType) : base($"Value {value} is not valid for property type {assignablePropertyType.FullName}.")
    {
    }

    public ValueFormatException(JsonNode value, Type assignablePropertyType) : base($"Value {value} is not valid for property type {assignablePropertyType.FullName}.")
    {
    }
}
