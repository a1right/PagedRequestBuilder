using Microsoft.Extensions.DependencyInjection;

namespace GenericFilters
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExpressionFilter(this IServiceCollection services)
        {
            services.AddScoped<IQueryBuilder, QueryBuilder>();
            services.AddScoped<IFilterBuilder, FilterBuilder>();
            return services;
        }
    }
}
