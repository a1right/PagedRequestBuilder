using EventStuff.Attributes;
using System;

namespace EventStuff.Models
{
    public class Example
    {
        [PagedRequestKeyAttribute("id")]
        public int Id { get; set; }
        [PagedRequestKeyAttribute("string")]
        public string String { get; set; }
        [PagedRequestKeyAttribute("date")]
        public DateTime Date { get; set; }
        [PagedRequestKeyAttribute("enum")]
        public ExampleEnum Enum { get; set; }
        [PagedRequestKeyAttribute("decimal")]
        public decimal Decimal { get; set; }
        [PagedRequestKeyAttribute("guid")]
        public Guid Guid { get; set; }
    }
    public enum ExampleEnum
    {
        ExampleOne,
        ExampleTwo,
        ExampleThree,
    }

    public class ExampleFilter : PagedRequestBase<Example>
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
    public class GetPagedExampleRequest : PagedRequestBase<Example>
    {
    }
}
