﻿using PagedRequestBuilder.Common.ValueParser.Models;
using PagedRequestBuilder.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser;

internal class PagedRequestValueParser : IPagedRequestValueParser
{
    private readonly IValueParseStrategyProvider _parseStrategyProvider;

    public PagedRequestValueParser(IValueParseStrategyProvider parseStrategyProvider)
    {
        _parseStrategyProvider = parseStrategyProvider;
    }
    public ValueParseResult GetValue(JsonNode? node, Type assignablePropertyType)
    {
        if (node is JsonArray array)
            return ParseArray(array, assignablePropertyType);

        if (node is JsonValue value)
            return _parseStrategyProvider.ForNode(node).Parse(value, assignablePropertyType);

        throw new ValueFormatException(node, assignablePropertyType);
    }

    private ValueParseResult ParseArray(JsonArray node, Type assignablePropertyType)
    {
        var parseResult = node
            .Select(x => GetValue(x, assignablePropertyType))
            .ToArray();

        var result = Array.CreateInstance(parseResult[0].ValueType, parseResult.Length);

        for (var index = 0; index < parseResult.Length; index++)
        {
            result.SetValue(parseResult[index].Value, index);
        }

        return new ValueParseResult(result, result.GetType());
    }
}

public interface IPagedRequestValueParser
{
    ValueParseResult GetValue(JsonNode? node, Type assignablePropertyType);
}
