using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Models.Filter
{
    public class FilterEntry
    {
        public string? Property { get; set; }
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

            var equals =
                Property == other.Property &&
                Value?.AsValue().ToJsonString() == other.Value?.AsValue().ToJsonString() &&
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
            if (Property is not null)
                return Property.GetHashCode();

            return base.GetHashCode();
        }
    }
}
