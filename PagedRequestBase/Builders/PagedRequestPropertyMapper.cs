using PagedRequestBuilder.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

internal class PagedRequestPropertyMapper : IPagedRequestPropertyMapper
{
    private static readonly Dictionary<Type, Dictionary<string, string?>> _typesRequestKeyToPropertyMaps = new();
    public string? MapRequestNameToPropertyName<T>(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return null;

        var containingType = typeof(T);
        if (_typesRequestKeyToPropertyMaps.ContainsKey(containingType))
            if (_typesRequestKeyToPropertyMaps[containingType].ContainsKey(propertyName))
                return _typesRequestKeyToPropertyMaps[containingType][propertyName];

        throw new NotImplementedException($"Type '{containingType.FullName}' do not contains paged request key '{propertyName}'");
    }

    public static void ScanPagedRequestKeys()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetTypes()
            .Where(t => t.GetProperties()
                .Any(p => p
                    .IsDefined(typeof(PagedRequestKeyAttribute), true)))).SelectMany(x => x).ToList();

        foreach (var type in types)
        {
            var keyToPropertyNamesMaps = type.GetProperties()
                .Where(p => p.IsDefined(typeof(PagedRequestKeyAttribute)))
                .Select(x => x.GetCustomAttribute<PagedRequestKeyAttribute>())
                .ToDictionary(key => key.RequestKey, value => value.TargetProperty);

            _typesRequestKeyToPropertyMaps.Add(type, keyToPropertyNamesMaps);
        }
    }
}

internal interface IPagedRequestPropertyMapper
{
    string? MapRequestNameToPropertyName<T>(string? propertyName);
}
