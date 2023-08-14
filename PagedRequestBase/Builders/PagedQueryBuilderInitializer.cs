namespace EventStuff.Builders
{

    public static class PagedQueryBuilder
    {
        public static void Initialize() => PagedRequestPropertyMapper.ScanPagedRequestKeys();
    }
}
