using System.Text.Json;

namespace EventStuff.Models.Filter
{
    public class FilterEntry
    {
        public string? Property { get; set; }
        public JsonDocument? Value { get; set; }
        public string? Operation { get; set; }
    }
}
