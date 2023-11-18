using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Models.Filter;

public struct FilterEntry
{
    public FilterEntry()
    {
        Property = string.Empty;
    }

    public string Property { get; set; }
    public JsonNode? Value { get; set; }
    public string? Operation { get; set; }
}

public class FilterEqualityComparers : IEqualityComparer<FilterEntry>
{
    public bool Equals(FilterEntry x, FilterEntry y)
    {
        return x.Value?.ToJsonString() == y.Value?.ToJsonString() &&
            x.Operation == y.Operation &&
            x.Property == y.Property;
    }

    public int GetHashCode(FilterEntry obj)
    {
        unchecked
        {
            var code = 0;
            if (obj.Operation != null)
                code += obj.Operation.GetHashCode();

            if (obj.Value != null)
                code += obj.Value.ToJsonString().GetHashCode();

            if (obj.Property != null)
                code += obj.Property.GetHashCode();

            return code == 0 ? base.GetHashCode() : code;
        }
    }
}
