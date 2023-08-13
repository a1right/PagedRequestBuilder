using EventStuff.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace EventStuff.Extensions
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
            return services;
        }
    }
}
