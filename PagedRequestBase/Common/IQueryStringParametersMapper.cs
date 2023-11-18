using PagedRequestBuilder.Models;

namespace PagedRequestBuilder.Common;
internal interface IQueryStringParametersMapper
{
    void MapQueryStringParams(ref PagedRequestBase? request);
}