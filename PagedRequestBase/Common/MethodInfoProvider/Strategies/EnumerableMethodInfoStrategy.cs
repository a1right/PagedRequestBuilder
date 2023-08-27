using PagedRequestBuilder.Constants;
using System;
using System.Linq;
using System.Reflection;

namespace PagedRequestBuilder.Common.MethodInfoProvider.Strategies;

internal class EnumerableMethodInfoStrategy : IMethodInfoStrategy
{
    public MethodInfo GetMethodInfo(string name, Type assignablePropertyType)
    {
        var enumerableOfType = assignablePropertyType.GetGenericArguments().First();
        return Get(name, enumerableOfType);
    }

    public MethodInfo Get(string name, Type enumerableOfType) => name switch
    {
        Strings.MethodInfoNames.Contains => GetStaticGenericMethod(name, enumerableOfType),

        _ => throw new NotImplementedException(name),
    };

    private MethodInfo GetStaticGenericMethod(string name, Type enumerableOfType) => typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.Name == name && x.GetParameters().Length == 2)
        .MakeGenericMethod(enumerableOfType);
}
