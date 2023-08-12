using EventStuff.Models;
using System;

public static class PagedRequestPropertyMapper
{
    public static string MapRequestNameToPropertyName<T>(string? propertyName)
    {
        var containingType = typeof(T);
        if (containingType == typeof(Example))
            return ExamplePropertyMap(propertyName);

        throw new NotImplementedException(containingType.Name);
    }

    private static string ExamplePropertyMap(string? propertyName) => propertyName switch
    {
        "id" => nameof(Example.Id),
        "string" => nameof(Example.String),
        "date" => nameof(Example.Date),
        "enum" => nameof(Example.Enum),
        "decimal" => nameof(Example.Decimal),
        "guid" => nameof(Example.Guid),
        _ => string.Empty
    };
}
