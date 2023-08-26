namespace PagedRequestBuilder.Models.Sorter;


public class SorterEntry
{
    public string? Property { get; set; }
    public bool? Descending { get; set; }

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

    public static bool operator ==(SorterEntry left, SorterEntry right)
    {
        if (left is null && right is not null)
            return false;

        if (left is null && right is null)
            return true;

        return left!.Equals(right);
    }

    public static bool operator !=(SorterEntry left, SorterEntry right)
    {
        if (left is null && right is not null)
            return true;

        if (left is null && right is null)
            return false;

        return !left!.Equals(right);
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
