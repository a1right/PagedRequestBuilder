﻿using Microsoft.Extensions.DependencyInjection;
using PagedRequestBuilder.Builders;
using PagedRequestBuilder.Cache;
using PagedRequestBuilder.Common;
using PagedRequestBuilder.Common.MethodInfoProvider;
using PagedRequestBuilder.Common.MethodInfoProvider.Strategies;
using PagedRequestBuilder.Common.ValueParser;
using PagedRequestBuilder.Common.ValueParser.Strategies;

namespace PagedRequestBuilder.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPagedQueryBuilder(this IServiceCollection services)
    {
        services.AddSingleton<IPagedRequestPropertyMapper, PagedRequestPropertyMapper>();
        services.AddBuilders();
        services.AddCaching();
        services.AddMethodInfo();
        services.AddValueParser();
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