using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    internal partial class PagedRequestValueParser
    {
        public class BoolParseStrategy : IValueParseStrategy
        {
            public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType) => new(value.GetValue<bool>(), typeof(bool));
        }
    }
}
