using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider
{
    public interface IMethodInfoProviderStrategy
    {
        MethodInfo GetMethodInfo(string name, Type assignablePropertyType);
    }
}
