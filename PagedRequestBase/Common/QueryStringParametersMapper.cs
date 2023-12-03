using PagedRequestBuilder.Models;
using PagedRequestBuilder.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common;
internal class QueryStringParametersMapper : IQueryStringParametersMapper
{
    public void MapQueryStringParams(PagedRequestBase request)
    {
        if (string.IsNullOrEmpty(request.Query))
            return;

        var queryTest = JsonNode.Parse(request.Query, new JsonNodeOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        var test = BuildFiltersTest(queryTest);

        foreach (var filter in test)
        {
            if (filter is ComplexFilterEntry complexFilter)
            {
                if (complexFilter.Chain is FilterChain.And)
                    foreach (var innerFilter in complexFilter.Filters)
                        request.Filters.Add(innerFilter);

                if (complexFilter.Chain is FilterChain.Or)
                    request.ComplexFilters.Add(complexFilter.Filters);
            }
        }
    }

    public List<FilterEntry> BuildFiltersTest(JsonNode args)
    {
        if (args is not JsonObject jObject)
            throw new NotImplementedException();

        List<FilterEntry> result = [];
        var index = -1;
        foreach (var prop in jObject)
        {
            ++index;
            var propertyName = prop.Key;

            if (propertyName == "$or" || propertyName == "$and")
            {
                result.Add(OrFilters((JsonObject)args, index));
                continue;
            }

            var value = args[propertyName];

            if (value is JsonValue or JsonArray)
            {
                var filter = new FilterEntry()
                {
                    Operation = propertyName,
                    Property = ((JsonObject)args.Root).Skip(index).First().Key,
                    Value = value
                };

                result.Add(filter);
                continue;
            }

            result.Add(BuildFilters(value));
        }

        return result;
    }

    //DONT MODIFY
    public FilterEntry BuildFilters(JsonNode args)
    {
        if (args is not JsonObject jObject)
            throw new NotImplementedException();

        var propertyName = jObject.Select(x => x.Key).First();

        if (propertyName == "$or" || propertyName == "$and")
            return OrFilters((JsonObject)args, 0);

        var value = args[propertyName];

        if (value is JsonValue or JsonArray)
        {
            var filter = new FilterEntry()
            {
                Operation = propertyName,
                Property = ((JsonObject)args.Root).First().Key,
                Value = value
            };

            return filter;
        }

        return BuildFilters(value);
    }

    public ComplexFilterEntry OrFilters(JsonObject args, int current)
    {
        var complexFilter = new ComplexFilterEntry();
        var propertyName = args.Skip(current).First().Key;
        if (propertyName == "$or")
            complexFilter.Chain = FilterChain.Or;

        var value = args[propertyName];

        if (value is not JsonArray or JsonObject)
            throw new InvalidOperationException();

        if (value is JsonObject jObject)
        {
            var newRoot = jObject.ToJsonString();
            var newNode = JsonNode.Parse(newRoot);
            if (((JsonObject)newNode).First().Key is "$or" or "$and")
                throw new NotImplementedException("Double nested boolean is not supported");
            complexFilter.Filters.Add(BuildFilters(newNode!));
        }
        else if (value is JsonArray jArray)
        {
            foreach (var node in jArray)
            {
                if (node is not JsonObject)
                    throw new NotImplementedException();

                var newRoot = node.ToJsonString();
                var newNode = JsonNode.Parse(newRoot);

                if (((JsonObject)newNode).Any(x => x.Key is "$or" or "$and"))
                    throw new NotImplementedException("Double nested boolean is not supported");

                complexFilter.Filters.Add(BuildFilters(newNode!));
            }
        }

        return complexFilter;
    }
}
