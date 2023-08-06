using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericFilters
{
    public class FilterBuilder : IFilterBuilder
    {
        public Filter BuildFilter(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return new Filter();

            var filter = JToken.Parse(request);

            return BuildFilter(filter);
        }

        public Filter BuildFilter(JToken filter)
        {
            if (filter.Type == JTokenType.Property && filter is JProperty prop)
            {
                var (@operator, value) = BuildOperatorAndValue(prop);
                return new SimpleFilter() { Property = prop.Name, Operator = @operator, Value = value };
            }

            if (filter.Type != JTokenType.Object)
                throw new NotImplementedException();

            var result = new List<Filter>();

            foreach (var property in ((JObject)filter).Properties())
            {
                if (property.Value.Type == JTokenType.String)
                    result.Add(new SimpleFilter() { Operator = "$eq", Property = property.Name, Value = property.Value.ToString() });
                else
                {
                    result.Add(BuildFilter(property));
                }
            }

            return new ComplexFilter() { Filters = result };
        }

        private (string @operator, string value) BuildOperatorAndValue(JProperty operatorAndValue)
        {
            var obj = (JObject)operatorAndValue.Value;
            var property = obj.Properties().Single();
            return (property.Name, property.Value.ToString());
        }
    }

    public interface IFilterBuilder
    {
        Filter BuildFilter(string filter);
    }
}
