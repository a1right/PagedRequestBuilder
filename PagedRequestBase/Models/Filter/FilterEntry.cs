using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Models.Filter;

public class FilterEntry
{
    public string? Property { get; set; }
    public JsonNode? Value { get; set; }
    public string? Operation { get; set; }
    public string[]? Nested { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null)
            return false;

        if (obj is not FilterEntry other)
            return false;

        var equals =
            Property == other.Property &&
            Value?.ToJsonString() == other.Value?.ToJsonString() &&
            Operation == other?.Operation;

        if (Nested is not null)
            equals = equals && Nested.OrderBy(x => x).SequenceEqual(other!.Nested.OrderBy(x => x));

        return equals;
    }

    public override int GetHashCode()
    {
        var hashCode = 0;
        unchecked
        {
            if (Property is not null)
                hashCode += Property.GetHashCode();

            if (Nested is not null)
                foreach (var nested in Nested)
                    hashCode += nested.GetHashCode();
        }

        return hashCode == 0 ? base.GetHashCode() : hashCode;
    }
}
