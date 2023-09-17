using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Models.Filter;

public class FilterEntry
{
    public string Property { get; set; } = string.Empty;
    public JsonNode? Value { get; set; }
    public string? Operation { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null)
            return false;

        if (obj is not FilterEntry other)
            return false;

        return
            Property == other.Property &&
            Value?.ToJsonString() == other.Value?.ToJsonString() &&
            Operation == other?.Operation;
    }

    public override int GetHashCode()
    {
        var hashCode = 0;
        unchecked
        {
            if (Property is not null)
                hashCode += Property.GetHashCode();
        }

        return hashCode == 0 ? base.GetHashCode() : hashCode;
    }
}
