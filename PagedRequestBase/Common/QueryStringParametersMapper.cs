using PagedRequestBuilder.Models;
using System.Text.Json;

namespace PagedRequestBuilder.Common;
internal class QueryStringParametersMapper : IQueryStringParametersMapper
{
    public void MapQueryStringParams(ref PagedRequestBase? request)
    {
        if (request is null)
            return;

        if (request.Value.Query is null)
            return;

        var options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var queryStringRequest = JsonSerializer.Deserialize<PagedRequestBase?>(request.Value.Query, options);
        if (queryStringRequest is null)
            return;

        request = queryStringRequest;
    }
}
