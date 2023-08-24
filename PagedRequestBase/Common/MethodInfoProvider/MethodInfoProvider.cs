using PagedRequestBuilder.Cache;
using System;
using System.Collections;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider
{
    internal class MethodInfoProvider : IMethodInfoProvider
    {
        private readonly IMethodInfoCache _methodInfoCache;

        public MethodInfoProvider(IMethodInfoCache methodInfoCache)
        {
            _methodInfoCache = methodInfoCache;
        }
        public MethodInfo GetMethodInfo(string name, Type assignablePropertyType)
        {
            var cached = _methodInfoCache.Get(name, assignablePropertyType);
            if (cached is not null)
                return cached;

            var strategy = GetStrategy(assignablePropertyType);
            var methodInfo = strategy.GetMethodInfo(name, assignablePropertyType);
            _methodInfoCache.Set(assignablePropertyType, name, methodInfo);
            return methodInfo;
        }

        private IMethodInfoProviderStrategy GetStrategy(Type assignablePropertyType)
        {
            if (assignablePropertyType == typeof(string))
                return new StringMethodInfoStrategy();

            if (typeof(IEnumerable).IsAssignableFrom(assignablePropertyType))
                return new EnumerableMethodInfoStrategy();

            if (assignablePropertyType.IsArray)
                return new ArrayMethodInfoStrategy();

            throw new NotImplementedException(assignablePropertyType.FullName);
        }
    }

    public interface IMethodInfoProvider
    {
        MethodInfo GetMethodInfo(string name, Type assignablePropertyType);
    }
}
