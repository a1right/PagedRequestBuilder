using Microsoft.Extensions.DependencyInjection;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.MethodInfoProvider;
using PagedRequestBuilder.Common.MethodInfoProvider.Strategies;
using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Common.ValueParser.Strategies;
using PagedRequestBuilder.Models;
using System;

namespace PagedRequestBuilder.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPagedQueryBuilder(this IServiceCollection services, Action<PaginationConfig>? config = null)
    {
        services.AddSingleton<IRequestPropertyMapper, RequestPropertyMapper>();
        services.AddBuilders();
        services.AddCaching();
        services.AddMethodInfo();
        services.AddValueParser();

        SetDefaultPagintationSettings(config);

        return services;
    }

    private static void SetDefaultPagintationSettings(Action<PaginationConfig>? config = null)
    {
        var options = new PaginationConfig();
        config?.Invoke(options);

        PaginationSetting.DefaultPageSize = options.DefaultPageSize < 1 ? Constants.DefaultPaginationSettings.DefaultPageSize : options.DefaultPageSize;
        PaginationSetting.DefaultPageNumber = options.DefaultPageNumber < 1 ? Constants.DefaultPaginationSettings.DefaultPageNumber : options.DefaultPageNumber;
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
        services.AddSingleton(typeof(IPagedQueryBuilder<>), typeof(PagedQueryBuilder<>));
        services.AddSingleton(typeof(IFilterBuilder<>), typeof(FilterBuilder<>));
        services.AddSingleton(typeof(ISorterBuilder<>), typeof(SorterBuilder<>));
        services.AddSingleton<IMethodCallExpressionBuilder, MethodCallExpressionBuilder>();
        return services;
    }

    private static IServiceCollection AddValueParser(this IServiceCollection services)
    {
        services.AddSingleton<IPagedRequestValueParser, PagedRequestValueParser>();
        services.AddSingleton<IValueParseStrategyProvider, ValueParseStrategyProvider>();
        services.AddSingleton<NumberParseStrategy>();
        services.AddSingleton<StringParseStrategy>();
        services.AddSingleton<BoolParseStrategy>();
        return services;
    }
}
