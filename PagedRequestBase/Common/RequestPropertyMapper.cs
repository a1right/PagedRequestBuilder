using PagedRequestBuilder.Attributes;
using PagedRequestBuilder.Constants;
using PagedRequestBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PagedRequestBuilder.Common;

internal class RequestPropertyMapper : IRequestPropertyMapper
{
    private static bool _initialized;
    private static readonly Dictionary<Type, Dictionary<string, string?>> _typesRequestKeyToPropertyMaps = new();
    public string? MapRequestNameToPropertyName<T>(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return null;

        var containingType = typeof(T);
        if (_typesRequestKeyToPropertyMaps.ContainsKey(containingType))
            if (_typesRequestKeyToPropertyMaps[containingType].ContainsKey(propertyName))
                return _typesRequestKeyToPropertyMaps[containingType][propertyName];

        throw new ArgumentException(Strings.Errors.Templates.TypeNotContainsPagedRequestKey.Format(containingType.FullName, propertyName));
    }

    public string? MapNestedRequestNameToPropertyName<T>(string? propertyName, Type nested)
    {
        if (string.IsNullOrEmpty(propertyName))
            return null;

        if (_typesRequestKeyToPropertyMaps.ContainsKey(nested))
            if (_typesRequestKeyToPropertyMaps[nested].ContainsKey(propertyName))
                return _typesRequestKeyToPropertyMaps[nested][propertyName];

        throw new ArgumentException(Strings.Errors.Templates.TypeNotContainsPagedRequestKey.Format(nested.FullName, propertyName));
    }

    public static void ScanPagedRequestKeys()
    {
        if (_initialized)
            return;

        var types = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetTypes()
            .Where(t => t.GetProperties()
                .Any(p => p
                    .IsDefined(typeof(PagedRequestKeyAttribute), true))))
            .SelectMany(x => x)
            .ToList();

        foreach (var type in types)
        {
            var keyToPropertyNamesMaps = type.GetProperties()
                .Where(p => p.IsDefined(typeof(PagedRequestKeyAttribute)))
                .Select(x => x.GetCustomAttribute<PagedRequestKeyAttribute>())
                .ToDictionary(key => key.RequestKey, value => value.TargetProperty);

            _typesRequestKeyToPropertyMaps.Add(type, keyToPropertyNamesMaps);
        }

        _initialized = true;
    }
}

public interface IRequestPropertyMapper
{
    string? MapRequestNameToPropertyName<T>(string? propertyName);
    string? MapNestedRequestNameToPropertyName<T>(string? propertyName, Type nested);
}
