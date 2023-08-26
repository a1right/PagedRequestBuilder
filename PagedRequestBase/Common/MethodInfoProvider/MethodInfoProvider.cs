using PagedRequestBuilder.Cache;
using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider;

internal class MethodInfoProvider : IMethodInfoProvider
{
    private readonly IMethodInfoCache _methodInfoCache;
    private readonly IMethodInfoStrategyProvider _strategyProvider;

    public MethodInfoProvider(IMethodInfoCache methodInfoCache, IMethodInfoStrategyProvider strategyProvider)
    {
        _methodInfoCache = methodInfoCache;
        _strategyProvider = strategyProvider;
    }
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType)
    {
        var cached = _methodInfoCache.Get(name, assignablePropertyType);
        if (cached is not null)
            return cached;

        var methodInfo = _strategyProvider
            .OfType(assignablePropertyType)
            .GetMethodInfo(name, assignablePropertyType);

        _methodInfoCache.Set(assignablePropertyType, name, methodInfo);

        return methodInfo;
    }
}

public interface IMethodInfoProvider
{
    MethodInfo GetMethodInfo(string name, Type assignablePropertyType);
}
