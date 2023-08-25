using System.Linq;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Models.Filter
{
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
                Value?.AsValue().ToJsonString() == other.Value?.AsValue().ToJsonString() &&
                Nested.OrderBy(x => x).SequenceEqual(other.Nested.OrderBy(x => x)) &&
                Operation == other?.Operation;

            return equals;
        }

        public static bool operator ==(FilterEntry left, FilterEntry right)
        {
            if (left is null && right is not null)
                return false;

            if (left is null && right is null)
                return true;

            return left!.Equals(right);
        }

        public static bool operator !=(FilterEntry left, FilterEntry right)
        {
            if (left is null && right is not null)
                return true;

            if (left is null && right is null)
                return false;

            return !left!.Equals(right);
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
}
