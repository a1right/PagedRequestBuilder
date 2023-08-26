using PagedRequestBuilder.Constant;
using System;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

internal class StringMethodInfoStrategy : IMethodInfoStrategy
{
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType) => name switch
    {
        Constants.MethodInfoNames.Contains => typeof(string).GetMethod(name, new[] { typeof(string) }),

        _ => throw new NotImplementedException(name),
    };
}
