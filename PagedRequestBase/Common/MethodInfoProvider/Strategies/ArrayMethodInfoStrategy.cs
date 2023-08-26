using PagedRequestBuilder.Constant;
using System;
using System.Linq;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

internal class ArrayMethodInfoStrategy : IMethodInfoStrategy
{
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType)
    {
        var arrayOfType = assignablePropertyType.GetElementType();
        return Get(name, arrayOfType);
    }

    public MethodInfo Get(string name, Type arrayOfType) => name switch
    {
        Constants.MethodInfoNames.Contains => GetStaticGenericMethod(name, arrayOfType),

        _ => throw new NotImplementedException(name),
    };

    private MethodInfo GetStaticGenericMethod(string name, Type arrayOfType) => typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.Name == name && x.GetParameters().Length == 2)
        .MakeGenericMethod(arrayOfType);
}
