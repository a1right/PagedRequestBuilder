using System.Linq;

namespace PagedRequestBuilder.Models.Sorter;

public class SorterEntry
{
    public string? Property { get; set; }
    public bool? Descending { get; set; }
    public string[]? Nested { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null)
            return false;

        if (obj is not SorterEntry other)
            return false;

        var equals =
            Property == other.Property &&
            Descending == other.Descending;

        if (Nested is not null)
            equals = equals && Nested.OrderBy(x => x).SequenceEqual(other.Nested.OrderBy(x => x));

        return equals;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = 0;
            if (Property is not null)
                hashCode = Property.GetHashCode();

            if (Descending is not null)
                hashCode += Descending.GetHashCode();

            return hashCode == 0 ? base.GetHashCode() : hashCode;
        }
    }
}
