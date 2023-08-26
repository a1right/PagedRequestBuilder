using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider
{
    public interface IMethodInfoStrategy
    {
        MethodInfo GetMethodInfo(string name, Type assignablePropertyType);
    }
}
