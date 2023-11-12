using PagedRequestBuilder.Models;

namespace PagedRequestBuilder.Common;
internal interface IQueryStringParametersMapper
{
    void MapQueryStringParams(PagedRequestBase request);
}