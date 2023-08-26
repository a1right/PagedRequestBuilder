using PagedRequestBuilder.Constant;
using System;
using System.Linq;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

internal class ArrayMethodInfoStrategy : IMethodInfoStrategy
{
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType) => name switch
    {
        Constants.MethodInfoNames.Contains => GetArrayMethod(name, assignablePropertyType),

        _ => throw new NotImplementedException(name),
    };
    private MethodInfo GetArrayMethod(string name, Type assignablePropertyType)
    {
        return typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.Name == name && x.GetParameters().Length == 2)
        .MakeGenericMethod(assignablePropertyType.GetElementType());
    }
}
