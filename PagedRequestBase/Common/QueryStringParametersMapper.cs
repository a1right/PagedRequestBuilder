using PagedRequestBuilder.Models;
using System.Text.Json;

namespace PagedRequestBuilder.Common;
internal class QueryStringParametersMapper : IQueryStringParametersMapper
{
    public void MapQueryStringParams(PagedRequestBase request)
    {
        if (request.Query is null)
            return;
        var options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var queryStringRequest = JsonSerializer.Deserialize<PagedRequestModel>(request.Query, options);
        if (queryStringRequest is null)
            return;

        request.Filters.AddRange(queryStringRequest.Filters);
        request.ComplexFilters.AddRange(queryStringRequest.ComplexFilters);
        request.Sorters.AddRange(queryStringRequest.Sorters);
        request.SortKeys.AddRange(queryStringRequest.SortKeys);
        request.Page = queryStringRequest.Page;
        request.Size = queryStringRequest.Size;
    }
}
