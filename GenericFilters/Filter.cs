using System.Collections.Generic;

namespace GenericFilters
{
    public class Filter
    {
    }

    public class SimpleFilter : Filter
    {
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public string? Property { get; set; }
    }

    public class ComplexFilter : Filter
    {
        public List<Filter>? Filters { get; set; } = new List<Filter>();
    }
}