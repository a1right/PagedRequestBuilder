using PagedRequestBuilder.Attributes;
using System;
using System.Collections.Generic;

namespace PagedRequestBuilder.Models
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
        [PagedRequestKey("decimals")]
        public List<decimal> Decimals { get; set; }
        [PagedRequestKey("ints")]
        public int[] Ints { get; set; }
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
        public List<decimal> Decimals { get; set; }
        public int[] Ints { get; set; }
    }
    public class GetPagedExampleRequest : PagedRequestBase<Example>
    {
    }
}
