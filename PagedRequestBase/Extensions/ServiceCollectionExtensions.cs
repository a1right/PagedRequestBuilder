using Microsoft.Extensions.DependencyInjection;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.MethodInfoProvider;

namespace PagedRequestBuilder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPagedQueryBuilder(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPagedQueryBuilder<>), typeof(PagedQueryBuilder<>));
            services.AddScoped(typeof(IFilterBuilder<>), typeof(FilterBuilder<>));
            services.AddScoped(typeof(ISorterBuilder<>), typeof(SorterBuilder<>));
            services.AddScoped<IPagedRequestValueParser, PagedRequestValueParser>();
            services.AddSingleton<IPagedRequestPropertyMapper, PagedRequestPropertyMapper>();
            services.AddSingleton(typeof(IQueryFilterCache<>), typeof(QueryFilterCache<>));
            services.AddSingleton(typeof(IQuerySorterCache<>), typeof(QuerySorterCache<>));
            services.AddSingleton<IMethodInfoCache, MethodInfoCache>();
            services.AddSingleton<IMethodInfoProvider, MethodInfoProvider>();
            services.AddSingleton<IMethodCallExpressionBuilder, MethodCallExpressionBuilder>();
            return services;
        }
    }
}
