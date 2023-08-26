using PagedRequestBuilder.Constant;
using System;
using System.Linq;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

internal class EnumerableMethodInfoStrategy : IMethodInfoStrategy
{
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType) => name switch
    {
        Constants.MethodInfoNames.Contains => GetLinqMethodInfo(name, assignablePropertyType),

        _ => throw new NotImplementedException(name),
    };

    private MethodInfo GetLinqMethodInfo(string name, Type assignablePropertyType)
    {
        return typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.Name == name && x.GetParameters().Length == 2)
        .MakeGenericMethod(assignablePropertyType.GetGenericArguments().First());
    }
}
