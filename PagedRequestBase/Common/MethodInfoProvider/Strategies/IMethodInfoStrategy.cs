using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

public interface IMethodInfoStrategy
{
    MethodInfo GetMethodInfo(string name, Type assignablePropertyType);
}
