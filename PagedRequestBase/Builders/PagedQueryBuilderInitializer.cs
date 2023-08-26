using PagedRequestBuilder.Common;

namespace PagedRequestBuilder.Builders;

public static class PagedQueryBuilder
{
    public static void Initialize() => RequestPropertyMapper.ScanPagedRequestKeys();
}
