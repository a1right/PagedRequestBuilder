using System;
using System.Collections.Concurrent;

namespace PagedRequestBuilder.Cache;

internal class PropertyTypeCache : IPropertyTypeCache
{
    private ConcurrentDictionary<Type, ConcurrentDictionary<string, Type>> _types = new();
    public Type GetOrAdd<T>(string? name)
    {
        var containingType = typeof(T);

        if (_types.TryGetValue(containingType, out var cachedProperties))
            if (cachedProperties.TryGetValue(name, out var cachedProperty))
                return cachedProperty;

        var propertyType = containingType.GetProperty(name).PropertyType;

        Set<T>(name, propertyType);

        return propertyType;
    }

    public void Set<T>(string? name, Type propertyType)
    {
        var containingType = typeof(T);

        if (!_types.TryGetValue(containingType, out var properties))
            _types.TryAdd(containingType, properties = new ConcurrentDictionary<string, Type>());

        properties.TryAdd(name, propertyType);
    }
}

public interface IPropertyTypeCache
{
    Type GetOrAdd<T>(string? name);
    void Set<T>(string? name, Type propertyType);
}
