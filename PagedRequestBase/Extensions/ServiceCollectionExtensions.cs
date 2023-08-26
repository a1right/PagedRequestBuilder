using Microsoft.Extensions.DependencyInjection;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common.MethodInfoProvider;
using PagedRequestBuilder.Common.ValueParser;

namespace PagedRequestBuilder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPagedQueryBuilder(this IServiceCollection services)
        {
            services.AddScoped<IPagedRequestValueParser, PagedRequestValueParser>();
            services.AddSingleton<IPagedRequestPropertyMapper, PagedRequestPropertyMapper>();
            services.AddBuilders();
            services.AddCaching();
            services.AddMethodInfo();
            return services;
        }

        private static IServiceCollection AddMethodInfo(this IServiceCollection services)
        {
            services.AddSingleton<IMethodInfoCache, MethodInfoCache>();
            services.AddSingleton<IMethodInfoProvider, MethodInfoProvider>();
            services.AddSingleton<IMethodInfoStrategyProvider, MethodInfoStrategyProvider>();
            services.AddSingleton<StringMethodInfoStrategy>();
            services.AddSingleton<ArrayMethodInfoStrategy>();
            services.AddSingleton<EnumerableMethodInfoStrategy>();
            return services;
        }

        private static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IQueryFilterCache<>), typeof(QueryFilterCache<>));
            services.AddSingleton(typeof(IQuerySorterCache<>), typeof(QuerySorterCache<>));
            services.AddSingleton<IMethodInfoCache, MethodInfoCache>();
            return services;
        }

        private static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPagedQueryBuilder<>), typeof(PagedQueryBuilder<>));
            services.AddScoped(typeof(IFilterBuilder<>), typeof(FilterBuilder<>));
            services.AddScoped(typeof(ISorterBuilder<>), typeof(SorterBuilder<>));
            services.AddSingleton<IMethodCallExpressionBuilder, MethodCallExpressionBuilder>();
            return services;
        }
    }
}
