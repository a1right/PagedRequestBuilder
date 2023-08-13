using System;
using System.Linq.Expressions;
using System.Text.Json;

namespace EventStuff.Models.Sorter
{

    public class SorterEntry
    {
        public string? Property { get; set; }
        public bool? Descending { get; set; }
    }
}
