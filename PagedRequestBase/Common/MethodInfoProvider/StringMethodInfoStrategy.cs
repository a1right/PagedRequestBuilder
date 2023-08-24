using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider
{
    public class StringMethodInfoStrategy : IMethodInfoProviderStrategy
    {
        public MethodInfo GetMethodInfo(string name, Type assignablePropertyType) => name switch
        {
            "Contains" => typeof(string).GetMethod(name, new[] { typeof(string) }),

            _ => throw new NotImplementedException(name),
        };
    }
}
