using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace PagedRequestBuilder.Cache
{
    public class MethodInfoCache : IMethodInfoCache
    {
        private ConcurrentDictionary<Type, ConcurrentDictionary<string, MethodInfo>> _methods = new();
        public MethodInfo? Get(string name, Type assignablePropertyType)
        {
            if (_methods.TryGetValue(assignablePropertyType, out var cachedMethods))
                if (cachedMethods is not null)
                {
                    if (cachedMethods.ContainsKey(name))
                        return cachedMethods[name];
                }

            return null;
        }

        public void Set(Type assignablePropertyType, string name, MethodInfo methodInfo)
        {
            if (!_methods.TryGetValue(assignablePropertyType, out var cachedMethods))
                _methods.TryAdd(assignablePropertyType, cachedMethods = new ConcurrentDictionary<string, MethodInfo>());

            cachedMethods.TryAdd(name, methodInfo);
        }
    }

    public interface IMethodInfoCache
    {
        MethodInfo? Get(string name, Type assignablePropertyType);
        void Set(Type assignablePropertyType, string name, MethodInfo methodInfo);
    }
}
