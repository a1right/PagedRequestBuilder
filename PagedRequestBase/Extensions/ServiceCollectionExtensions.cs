using EventStuff.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace EventStuff.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static int DefaultPageSize { get; private set; }
        internal static int DefaultPageNumber { get; private set; }
        public static IServiceCollection AddPagedQueryBuilder(this IServiceCollection services, int defaultPageSize, int defaultPageNumber)
        {
            services.AddScoped(typeof(IPagedQueryBuilder<>), typeof(PagedQueryBuilder<>));
            services.AddScoped(typeof(IFilterBuilder<>), typeof(FilterBuilder<>));
            services.AddScoped(typeof(ISorterBuilder<>), typeof(SorterBuilder<>));
            services.AddScoped<IPagedRequestValueParser, PagedRequestValueParser>();
            services.AddSingleton<IPagedRequestPropertyMapper, PagedRequestPropertyMapper>();

            DefaultPageNumber = defaultPageNumber < 1 ? defaultPageNumber : 1;
            DefaultPageSize = defaultPageSize <= 0 ? defaultPageSize : 10;

            return services;
        }
    }
}
