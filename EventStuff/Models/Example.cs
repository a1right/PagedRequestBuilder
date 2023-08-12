using System;

namespace EventStuff.Models
{
    public class Example
    {
        public int Id { get; set; }
        public string String { get; set; }
        public DateTime Date { get; set; }
        public ExampleEnum Enum { get; set; }
        public decimal Decimal { get; set; }
        public Guid Guid { get; set; }
    }
    public enum ExampleEnum
    {
        ExampleOne,
        ExampleTwo,
        ExampleThree,
    }

    public class ExampleFilter : GetPagedRequestBase<Example>
    {
    }

    public class ExampleDto
    {
        public string String { get; set; }
        public DateTime Date { get; set; }
        public ExampleEnum Enum { get; set; }
        public decimal Decimal { get; set; }
        public Guid Guid { get; set; }
    }
}
