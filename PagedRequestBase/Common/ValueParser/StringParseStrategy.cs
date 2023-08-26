using PagedRequestBuilder.Common.ValueParser.Models;
using System;
using System.Text.Json.Nodes;

namespace PagedRequestBuilder.Common.ValueParser
{
    public class StringParseStrategy : IValueParseStrategy
    {
        public ValueParseResult GetValue(JsonValue value, Type assignablePropertyType)
        {
            if (DateTime.TryParse(value.GetValue<string>(), out var dateTime))
                return new ValueParseResult(dateTime.ToUniversalTime(), typeof(DateTime));

            if (Guid.TryParse(value.GetValue<string>(), out var guid))
                return new ValueParseResult(guid, typeof(Guid));

            return new ValueParseResult(value.GetValue<string>(), typeof(string));
        }
    }
}
