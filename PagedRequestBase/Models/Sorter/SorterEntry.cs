using System.Collections.Generic;

namespace PagedRequestBuilder.Models.Sorter;

public struct SorterEntry
{
    public SorterEntry(string property, bool descending = false)
    {
        Property = property;
        Descending = descending;
    }

    public string Property { get; set; } = string.Empty;
    public bool Descending { get; set; }

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

        return equals;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = 0;
            if (Property is not null)
                hashCode = Property.GetHashCode();

            hashCode += Descending.GetHashCode();

            return hashCode == 0 ? base.GetHashCode() : hashCode;
        }
    }
}

public class SorterEntryEqualityComparer : IEqualityComparer<SorterEntry>
{
    public bool Equals(SorterEntry x, SorterEntry y)
    {
        return x.Property == y.Property &&
            x.Descending == y.Descending;
    }

    public int GetHashCode(SorterEntry obj)
    {
        unchecked
        {
            var hashCode = 0;
            if (obj.Property is not null)
                hashCode = obj.Property.GetHashCode();

            hashCode += obj.Descending.GetHashCode();

            return hashCode == 0 ? base.GetHashCode() : hashCode;
        }
    }
}
